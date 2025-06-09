using aZynEManager.CustomControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using aZynEManager.EF.CinemaPatrons;
using System.Threading;
using Cinex.Core.Entities;
using Cinex.WinForm;
using System.Collections.Immutable;

namespace aZynEManager.EF.Cinemas
{
    public partial class frmCinemaPatrons : Form
    {
        private readonly int _userId;
        private readonly int _cinemaId;
        private List<CinemaPatronDto> _cinemaPatronDtoDataSource;

        public frmCinemaPatrons(int userId, int cinemaId)
        {
            _userId = userId;
            _cinemaId = cinemaId;

            InitializeComponent();
        }

        private async void frmCinemaPatrons_Load(object sender, EventArgs e)
        {
            datagridCinemas.AutoGenerateColumns = false;
            datagridCinemaPatrons.AutoGenerateColumns = false;

            var cbHeaderIsAccomodated = new DatagridViewCheckBoxHeaderCell("Accomodated?");
            Column8.HeaderCell = cbHeaderIsAccomodated;
            cbHeaderIsAccomodated.OnCheckBoxClicked += new CheckBoxClickedHandler(cbHeaderIsAccomodated_OnCheckBoxClicked);

            var cbHeaderIsDefault = new DatagridViewCheckBoxHeaderCell("Is Default?");
            Column4.HeaderCell = cbHeaderIsDefault;
            cbHeaderIsDefault.OnCheckBoxClicked += new CheckBoxClickedHandler(cbHeaderIsDefault_OnCheckBoxClicked);
            
            await ReloadCinemas();
        }

        private async Task ReloadCinemas() => await LoadCinemas()
            .ContinueWith((a) =>
            {
                datagridCinemas_CellClick(sender: datagridCinemas,
                    e: new DataGridViewCellEventArgs(columnIndex: Column1.Index, rowIndex: datagridCinemas.CurrentRow?.Index ?? -1));
            }, TaskScheduler.FromCurrentSynchronizationContext());

        private async Task<List<Cinema>> GetCinemas()
        {
            var cinemaDataService = DependencyInjectionHelper.GetCinemaDataService;
            if(_cinemaId > 0) return (await cinemaDataService.GetAllAsync()).Where(t => t.Id == _cinemaId).ToList();
            return (await cinemaDataService.GetAllAsync()).ToList();
        }

        private async Task LoadCinemas()
        {
            datagridCinemas.BindingContext = new BindingContext();
            datagridCinemas.DataSource = await GetCinemas();
        }

        private Cinema GetSelectedCinema => datagridCinemas?.CurrentRow == null ? null : datagridCinemas?.CurrentRow.DataBoundItem as Cinema;

        private async Task<List<Patron>> GetPatrons()
        {
            var patronDataService = DependencyInjectionHelper.GetPatronDataService;
            return (await patronDataService.GetAllAsync())
                .OrderBy(c => c.Code)
                .ThenBy(c => c.Name)
                .ToList();
        }

        private async Task LoadCinemaPatrons()
        {
            var accomodatedHeaderCell = Column8.HeaderCell as DatagridViewCheckBoxHeaderCell;
            accomodatedHeaderCell.Unset();
            var isDefaultHeaderCell = Column4.HeaderCell as DatagridViewCheckBoxHeaderCell;
            isDefaultHeaderCell.Unset();

            var cinema = GetSelectedCinema;
            var cinemaPatrons = cinema?.Patrons.ToList();
            var cinemaDefaultPatrons = cinema?.DefaultPatrons.ToList();
            var patrons = await GetPatrons();

            var dataSource = patrons
                .Select(p => new CinemaPatronDto(patron: p,
                    cinemaPatron: cinemaPatrons.FirstOrDefault(cp => cp.PatronId == p.Id),
                    defaultPatron: cinemaDefaultPatrons.FirstOrDefault(cpd => cpd.PatronId == p.Id)))
                .ToList();

            datagridCinemaPatrons.BindingContext = new BindingContext();
            datagridCinemaPatrons.DataSource = dataSource;

            _cinemaPatronDtoDataSource = dataSource;
        }

        private void datagridCinemas_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private async void datagridCinemas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (datagridCinemas.CurrentRow == null) return;

            await LoadCinemaPatrons().
                ContinueWith((antecedent) => {
                    //MessageBox.Show(text: "text", caption: "caption", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async void ToolStripButtonSave_Click(object sender, EventArgs e)
        {
            datagridCinemaPatrons.EndEdit();
            ToolStripButtonSave.Enabled = false;

            var cinema = GetSelectedCinema;
            //var cinemaPatronsDto = datagridCinemaPatrons.Rows.OfType<DataGridViewRow>()
            //    .Select(r => GetPatron(r))
            //    .ToList();
            var cinemaPatronsDto = _cinemaPatronDtoDataSource;
            cinema.AddPatrons(cinemaPatronsDto.Where(t => t.IsAccomodated).Select(t => t.Patron).ToList());
            cinema.RemovePatrons(cinemaPatronsDto.Where(t => !t.IsAccomodated).Select(t => t.Patron).ToList());

            cinema.AddDefaultPatrons(cinemaPatronsDto.Where(t => t.IsDefault).Select(t => t.Patron).ToList());
            cinema.RemoveDefaultPatrons(cinemaPatronsDto.Where(t => !t.IsDefault).Select(t => t.Patron).ToList());

            var cinemaPatronDataService = DependencyInjectionHelper.GetCinemaPatronDataService;
            var cinemaPatronDefaultDataService = DependencyInjectionHelper.GetCinemaPatronDefaultDataService;

            var allSaveTasks = Task.WhenAll(
                cinemaPatronDataService.SaveManyAsync(entities: cinema.Patrons.ToList(), userId: _userId),
                cinemaPatronDefaultDataService.SaveManyAsync(entities: cinema.DefaultPatrons.ToList(), userId: _userId)
            );

            await allSaveTasks
                .ContinueWith(async (a) => {

                    MessageBox.Show(
                        text: "Done",
                        caption: "Finish",
                        buttons: MessageBoxButtons.OK,
                        icon: MessageBoxIcon.Information);

                    await ReloadCinemas();

                    ToolStripButtonSave.Enabled = true;
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void ToolStripButtonCancel_Click(object sender, EventArgs e)
        {
            datagridCinemas_CellClick(sender: datagridCinemas,
                e: new DataGridViewCellEventArgs(columnIndex: Column1.Index, rowIndex: datagridCinemas.CurrentRow?.Index ?? -1));
        }

        private void cbHeaderIsAccomodated_OnCheckBoxClicked(bool state)
        {
            var dataSource = datagridCinemaPatrons.Rows.OfType<DataGridViewRow>()
                .Select(r => GetPatron(r))
                .OrderBy(c => c.Code)
                .ThenBy(c => c.Name)
                .ToList();

            dataSource
                .Where(p => p.IsAccomodated != state)
                .ToList()
                .ForEach(p =>
                {
                    p.IsAccomodated = state;
                });

            datagridCinemaPatrons.DataSource = dataSource;

            datagridCinemaPatrons.EndEdit();
            datagridCinemaPatrons.Refresh();
        }

        private void cbHeaderIsDefault_OnCheckBoxClicked(bool state)
        {
            var dataSource = datagridCinemaPatrons.Rows.OfType<DataGridViewRow>()
                .Select(r => GetPatron(r))
                .OrderBy(c => c.Code)
                .ThenBy(c => c.Name)
                .ToList();

            dataSource
                .Where(p => p.IsDefault != state)
                .ToList()
                .ForEach(p =>
                {
                    p.IsDefault = state;
                });

            datagridCinemaPatrons.DataSource = dataSource;

            datagridCinemaPatrons.EndEdit();
            datagridCinemaPatrons.Refresh();
        }

        private CinemaPatronDto GetPatron(DataGridViewRow r) => r?.DataBoundItem as CinemaPatronDto;

        private void datagridCinemaPatrons_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            datagridCinemaPatrons.Rows[e.RowIndex].HeaderCell.Value = $"{e.RowIndex + 1}";
        }

        private void datagridCinemaPatrons_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var cinemaPatronDto = GetPatron(datagridCinemaPatrons.CurrentRow);
            if (cinemaPatronDto == null) return;

            Console.WriteLine("IsAccomodated: {0}, IsDefault: {1}", cinemaPatronDto.IsAccomodated, cinemaPatronDto.IsDefault);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            var searchDataSource = Enumerable.Empty<CinemaPatronDto>();

            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                datagridCinemaPatrons.DataSource = _cinemaPatronDtoDataSource;
                return;
            }

            if (!string.IsNullOrEmpty(txtSearch.Text))
                searchDataSource = _cinemaPatronDtoDataSource
                    .Where(t => string.Concat(t.Code, t.Name, $"{t.OfficialUnitPrice}").SimilarTo(txtSearch.Text))
                    .ToList();

            if (rbtnAccomodated.Checked) searchDataSource = searchDataSource
                    .Where(t => t.IsAccomodated)
                    .ToList();

            if (rbtnDefault.Checked) searchDataSource = searchDataSource
                    .Where(t => t.IsDefault)
                    .ToList();

            datagridCinemaPatrons.DataSource = searchDataSource;
        }

        private void rbtnAll_CheckedChanged(object sender, EventArgs e)
        {
            if (!rbtnAll.Checked) return;

            txtSearch_TextChanged(txtSearch, e);
        }

        private void rbtnAccomodated_CheckedChanged(object sender, EventArgs e)
        {
            if (!rbtnAccomodated.Checked) return;

            txtSearch_TextChanged(txtSearch, e);
        }

        private void rbtnDefault_CheckedChanged(object sender, EventArgs e)
        {
            if (!rbtnDefault.Checked) return;

            txtSearch_TextChanged(txtSearch, e);
        }
    }
}
