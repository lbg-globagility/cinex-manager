using System;
using System.Collections.Generic;
using System.Text;
using MapWinGIS;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Globalization;
using System.Windows.Forms;
using System.Threading;
using System.Data.OleDb;
using System.Data;
using Amellar.Common.DataConnector;
using System.Drawing.Drawing2D;
using MapWinUtility;
using System.Linq;
using System.ComponentModel;
using MapWindow.Interfaces;
//using MapWinGeoProc;
using System.Diagnostics;
using Google.KML;
//using MapWindow.Data;

namespace AmellarGISParcelMapping
{
    /// <summary>
    /// common class for parcelmapping
    /// </summary>
    public class clscommon
    {
        /// <summary>
        /// problem #1 generated point does'nt always falls in the polygon shape upon loading of the building point ver 2
        /// </summary>
        public string c_serverdate = String.Empty;
        public string c_currentyear= String.Empty;
        public frmMain frmM;

        ///declared variables for making hold of drawing rubberband lines(start)
        ///list of variables are used for subdividing land parcel

        //Holds where the use click to start the rubber band in Map Coords
        public MapWinGIS.Point _startPt = null;

        //Holds where the location of the mouses last location Map Coords
        //Needed so we can invalidate the old line before drawing the new
        public MapWinGIS.Point _oldMousePt = null;

        //Pen used to draw the line set it to something you line
        public Pen _LinePen = new System.Drawing.Pen(System.Drawing.Color.Red, 2F);
        public bool _Measuring = true;
        
        ///declared variables for making hold of drawing rubberband lines(end)

        ///declared for subd and cons
        public int draw_hndl = 0;
        public int draw_hndl1 = 0;
        public int draw_hndl2 = 0;
        public int drawsw = 0;
        //MapWinGIS.Point m_prevPt = new MapWinGIS.Point();

        //declaration of forms
        frmSubdivideLand2 frmsubd = null;
        frmConsLand frmcons = null;
        frmGoogleEarthViewer frmgooglekml = null;
        frmValuation frmvaluation = null;

        private static string gErrorMsg = ""; //added nov 12 2013

        public void loadShapeFileBoundary(string strShapeFileName, string strLayerName, bool isFill, UInt32 fillColor, UInt32 lineColor, float lineWidth, bool isZoomExt, string strLabelFld)
        {
            Shapefile MyShapeFile = new MapWinGIS.Shapefile();
            if (File.Exists(strShapeFileName) == true)
            {
                MyShapeFile.Open(strShapeFileName, null);

                frmM.hndMap = frmM.legend1.Layers.Add(MyShapeFile, true);
                if (isFill == true)
                {
                    frmM.axMap.set_ShapeLayerFillColor(frmM.hndMap, fillColor);
                }
                else
                {
                    frmM.axMap.set_ShapeLayerFillColor(frmM.hndMap, (UInt32)(System.Drawing.ColorTranslator.ToOle(Color.Transparent)));
                }

                frmM.axMap.set_ShapeLayerDrawFill(frmM.hndMap, true);
                frmM.axMap.set_ShapeLayerLineColor(frmM.hndMap, lineColor);
                frmM.axMap.set_ShapeLayerLineWidth(frmM.hndMap, lineWidth);
                frmM.legend1.Map.set_LayerName(frmM.hndMap, strLayerName);

                frmM.legend1.Layers.ItemByHandle(frmM.hndMap).Expanded = true;
                frmM.legend1.Layers.ItemByHandle(frmM.hndMap).Refresh();
                if (isZoomExt == true)
                    frmM.axMap.ZoomToLayer(frmM.hndMap);
                //frmM.hndMap += 1;
            }
        }

        public void loadShapeFileUnique(string strShapeFileName, string strLayerName, string FldName, UInt32 lineColor, float lineWidth, bool isZoomExt)
        {
            Shapefile MyShapeFile = new MapWinGIS.Shapefile();
            if (File.Exists(strShapeFileName) == true)
            {
                MyShapeFile.Open(strShapeFileName, null);

                int m_drawingHandle = frmM.axMap.NewDrawing(tkDrawReferenceList.dlScreenReferencedList);
                bool setLegend = colorByUniqueBreaksIII(MyShapeFile, FldName, strLayerName);

                frmM.axMap.Redraw();
            }
        }
        /// <summary>
        /// unique values simple fill per shape
        /// </summary>
        /// <param name="sf"></param>
        /// <param name="FldName"></param>
        /// <param name="strLayerName"></param>
        /// <returns></returns>
        public MapWinGIS.Shapefile uniquecategories(MapWinGIS.Shapefile sf, string FldName, string strLayerName)
        {
            int fldIdx = sf.Table.get_FieldIndexByName(FldName);
            sf.Categories.Generate(fldIdx, tkClassificationType.ctUniqueValues, 0);

            ColorScheme scheme = new ColorScheme();
            scheme.SetColors2(tkMapColor.Black, tkMapColor.White);
            sf.Categories.ApplyColorScheme(tkColorSchemeType.ctSchemeRandom, scheme);

            sf.Categories.Caption = FldName.ToUpper();
            sf.Categories.ApplyExpressions();

            sf.DefaultDrawingOptions.FillColor = (UInt32)(ColorTranslator.ToOle(Color.Transparent));
            sf.DefaultDrawingOptions.LineColor = (UInt32)(ColorTranslator.ToOle(Color.LightGray));

            sf.DefaultDrawingOptions.FillVisible = false;
            sf.DefaultDrawingOptions.LineVisible = false;
            sf.DefaultDrawingOptions.VerticesVisible = false;

            return sf;
        }

        /// <summary>
        /// unique values gradient legend per shape
        /// </summary>
        /// <param name="sf"></param>
        /// <param name="FldName"></param>
        /// <param name="strLayerName"></param>
        /// <returns></returns>
        public MapWinGIS.Shapefile uniquecategories3(MapWinGIS.Shapefile sf, string FldName, string strLayerName)
        {
            MapWinGIS.Utils utils = new UtilsClass();
            int fldIdx = sf.Table.get_FieldIndexByName(FldName);

            sf.DefaultDrawingOptions.FillType = tkFillType.ftGradient;
            sf.DefaultDrawingOptions.FillTransparency = (float)255;
            sf.DefaultDrawingOptions.FillGradientType = tkGradientType.gtLinear;
            sf.DefaultDrawingOptions.FillGradientBounds = tkGradientBounds.gbPerShape;
            sf.DefaultDrawingOptions.FillRotation = 0.00;

            sf.Categories.Generate(fldIdx, tkClassificationType.ctUniqueValues, 0);
            ColorScheme scheme = new ColorScheme();

            scheme.SetColors2(tkMapColor.Black, tkMapColor.White);
            sf.Categories.ApplyColorScheme(tkColorSchemeType.ctSchemeRandom, scheme);

            sf.Categories.Caption = FldName.ToUpper();
            sf.Categories.ApplyExpressions();

            sf.DefaultDrawingOptions.FillVisible = false;
            sf.DefaultDrawingOptions.LineVisible = false;
            sf.DefaultDrawingOptions.VerticesVisible = false;
            return sf;
        }

        bool colorByUniqueBreaksIII(MapWinGIS.Shapefile sf, string FldName, string strLayerName)
        {
            setCategories(sf, FldName);

            sf.DefaultDrawingOptions.FillColor = (UInt32)(ColorTranslator.ToOle(Color.Transparent));
            sf.DefaultDrawingOptions.LineColor = (UInt32)(ColorTranslator.ToOle(Color.LightGray));

            sf.DefaultDrawingOptions.FillVisible = false;
            sf.DefaultDrawingOptions.LineVisible = false;
            sf.DefaultDrawingOptions.VerticesVisible = false;

            frmM.hndMap = frmM.legend1.Layers.Add(sf, true);

            frmM.legend1.Map.set_LayerName(frmM.hndMap, strLayerName);
            frmM.legend1.Layers.ItemByHandle(frmM.hndMap).Refresh();

            return true;
        }

        public void setCategories(MapWinGIS.Shapefile sf, string FldName)
        {
            ShapefileColorScheme cs = new ShapefileColorScheme();
            int fldIdx = -1;
            for (int i = 0; i < sf.NumFields; i++)
            {
                if (sf.get_Field(i).Name.ToUpper() == FldName.ToUpper())
                {
                    fldIdx = i;
                    break;
                }
            }

            Hashtable ht = new Hashtable();
            object val;


            ShapefileCategory cat = new ShapefileCategory();
            sf.Categories.Clear();
            for (int i = 0; i < sf.NumShapes; i++)
            {
                val = sf.get_CellValue(fldIdx, i);
                if (ht.ContainsKey(val) == false)
                    ht.Add(val, val);
            }
            string[] arr;
            arr = new String[ht.Count];
            ht.Values.CopyTo(arr, 0);
            Array.Sort(arr);

            for (int ii = 0; ii < arr.Length; ii++)
            {
                string strcolor = String.Empty;
                //if (c_collCategory.ContainsKey(arr[ii].ToUpper()))
                //{
                //    strcolor = c_collCategory[arr[ii].ToUpper()];
                //}
                switch (arr[ii].ToUpper())
                {
                    case("RESIDENTIAL"):
                        strcolor = "YELLOW";
                        break;
                    case("AGRICULTURAL"):
                        strcolor = "GREEN";
                        break;
                    case("COMMERCIAL"):
                        strcolor = "RED";
                        break;
                    case("INDUSTRIAL"):
                        strcolor = "DARKVIOLET";
                        break;
                    case("MINERAL"):
                        strcolor = "BROWN";
                        break;
                    case("SPECIAL"):
                        strcolor = "CHOCOLATE";
                        break;
                    default:
                        strcolor = "TRANSPARENT";
                        break;
                }
                Color ccolor = Color.FromName(strcolor);
                if (ccolor != null)
                {
                    cat = sf.Categories.Add(arr[ii].ToUpper());
                    cat.Expression = "[Stall_type] = \"" + arr[ii].ToUpper() + "\"";
                    cat.DrawingOptions.FillColor = (uint)System.Drawing.ColorTranslator.ToOle(ccolor);
                }
            }
            sf.Categories.Caption = FldName.ToUpper();
            sf.Categories.ApplyExpressions();
        }

        public void setlabel(Shapefile sf, int Handle, int fldIndex)
        {
            //Labels lbl = frmM.axMap.get_DrawingLabels(Handle);//sf.Labels;
            MapWinGIS.Labels lbl = frmM.legend1.Map.get_DrawingLabels(Handle);
            if (lbl == null)
            {
                labelPoly(lbl,sf, Handle, fldIndex);
            }
            else
            {
                if (lbl.Visible == false)
                    lbl.Visible = true;
                else
                {
                    lbl.Visible = false;
                    //frmM.axMap.ClearDrawingLabels(Handle);
                    //frmM.axMap.ClearLabels(Handle);
                }
            }
            

            //if (togglecontrol == 0)
            //{
            //    labelPoly(sf,Handle, fldIndex);
            //    togglecontrol = 1;
            //}
            //else
            //{
            //    frmM.axMap.ClearLabels(Handle);
            //    togglecontrol = 0;
            //}

            
            frmM.axMap.Redraw();
            frmM.axMap.Refresh();
        }

        public void labelPoly(Labels lbl, Shapefile sf, int hndl, int intFldColumn)
        {
            //long icount = 0;

            if (lbl != null)
                lbl.Clear();
            else
            {
                //int drawHandle = frmM.axMap.NewDrawing(tkDrawReferenceList.dlScreenReferencedList);
                lbl = frmM.axMap.get_DrawingLabels(hndl);
                //icount = lbl.Generate("BRGYNAME", tkLabelPositioning.lpCentroid, false);
                //lbl = sf.Labels;//labels;
            }

            if (lbl != null)
                lbl.Alignment = tkLabelAlignment.laCenter;

            lbl = sf.Labels;
            lbl.VerticalPosition = tkVerticalPosition.vpAboveAllLayers;

            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;
            Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            //Shapefile sf = (Shapefile)axMap.get_GetObject(hndl);//hndMap2

            //how to get the layer name
            //frmM.legend1.Map.get_LayerName(hndl).ToUpper();
            MapWinGIS.Shape sh = null;

            string strFont = String.Empty;
            int intFontSize = 0;
            string strFontColor = String.Empty;

            switch (frmM.legend1.Map.get_LayerName(hndl).ToUpper())
            {
                case "BARANGAY BOUNDARY":
                    strFont = "ARIAL";
                    intFontSize = 7;
                    strFontColor = "RED";
                    break;
                default:
                    strFont = "Arial";
                    intFontSize = 7;
                    strFontColor = "BLACK";
                    break;
            }

            frmM.axMap.LayerFont(hndl, textInfo.ToTitleCase(strFont), intFontSize);
            frmM.axMap.LayerFontEx(hndl, textInfo.ToTitleCase(strFont), intFontSize, true, false, false);
            frmM.axMap.set_LayerLabelsOffset(hndl, 2);
            frmM.axMap.set_LayerLabelsShadow(hndl, true);

            frmM.axMap.ClearLabels(hndl);
            frmM.axMap.ClearDrawingLabels(hndl);
            lbl.Clear();
            for (int i = 0; i < sf.NumShapes; i++)
            {
                //Set the text for this shape
                sh = sf.get_Shape(i);
                String strVal = sf.get_CellValue(intFldColumn, i).ToString();

                MapWinGIS.Point pnt = sh.InteriorPoint;//.Centroid;
                Color ccolor = Color.FromName(strFontColor);
                lbl.FontColor = (UInt32)System.Drawing.ColorTranslator.ToOle(ccolor);
                lbl.AddLabel(strVal, pnt.x, pnt.y, 0.0, -1);
                ////old method
                ////Set the x and y coordinates for this label to be the min x and y coordinates of this shape
                //Extents ext = sf.QuickExtents(i);
                ////Calculate the x position for the label
                //Double x = sh.Extents.xMin +
                //    (sh.Extents.xMax - sh.Extents.xMin) / 2;
                ////Calculate the y postion for the label
                //Double y = sh.Extents.yMin +
                //    (sh.Extents.yMax - sh.Extents.yMin) / 2;
                ////add the label on the map
                ////Add the label to the layer by the shape centering the text
                //frmM.axMap.MultilineLabels = true;
                //Color ccolor = Color.FromName(strFontColor);
                //frmM.axMap.AddLabel(hndl, strVal, (UInt32)System.Drawing.ColorTranslator.ToOle(ccolor), x, y, MapWinGIS.tkHJustification.hjCenter);
                

            }
            //frmM.axMap.set_DrawingLabels(hndl, lbl);
            frmM.axMap.set_LayerLabelsVisible(hndl, true);
            frmM.axMap.set_LayerLabelsScale(hndl, false);

            sf.Labels.Synchronized = true;

            //
            Cursor.Current = System.Windows.Forms.Cursors.Default;
        }

        public void labelLine(Shapefile sf, int intFldColumn, int hndl)
        {
            try
            {
                CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                TextInfo textInfo = cultureInfo.TextInfo;
                Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                if (frmM.legend1.Map.get_LayerName(hndl).ToUpper() == "")
                {
                    MessageBox.Show("No selected layer found, please check.", "Verification");
                    return;
                }
                MapWinGIS.Shape sh = null;
                Labels lbl = sf.Labels;
                frmM.axMap.ClearLabels(hndl);
                string strcolor = String.Empty;
                if (frmM.legend1.Map.get_LayerName(hndl).ToUpper() == "ROAD NETWORK")
                    strcolor = "RED";
                else
                    strcolor = "BLACK";

                for (int i = 0; i < sf.NumShapes; i++)
                {
                    try
                    {
                        //Set the text for this shape
                        sh = sf.get_Shape(i);
                        String strVal = sf.get_CellValue(intFldColumn, i).ToString();

                        Double x = (sh.Extents.xMax + sh.Extents.xMin) / 2;
                        Double y = (sh.Extents.yMax + sh.Extents.yMin) / 2;

                        Double x1 = sh.Extents.xMax;
                        Double x2 = sh.Extents.xMin;
                        Double y1 = sh.Extents.yMax;
                        Double y2 = sh.Extents.yMin;
                        Double angle = (y2 - y1) / (x2 - x1);
                        angle = Math.Atan(angle);
                        angle = (angle * 180) / Math.PI;

                        //Add the label to the layer by the shape centering the text
                        frmM.axMap.MultilineLabels = true;
                        Color ccolor = Color.FromName(strcolor);
                        lbl.AddLabel(strVal, x, y, angle, i);

                        sf.Labels.Synchronized = true;
                    }
                    catch (Exception)
                    {
                        //MessageBox.Show(err.Message);
                        continue;
                        //throw new Exception();

                    }
                }

                if (Path.GetFileNameWithoutExtension(sf.Filename.ToUpper()) == "ROADNETWORK")
                {
                    //lbl.FontName = c_collLayerFont[hndl].ToString().ToUpper();
                    //lbl.FontSize = Convert.ToInt32(c_collLayerFontSize[hndl].ToString());
                    //lbl.FontColor = (UInt32)(ColorTranslator.ToOle(Color.FromName(c_collLayerFontClr[hndl].ToString())));
                    //should be table based under utilitiesinitialization
                    lbl.FontName = "ARIAL";
                    lbl.FontSize = 8;
                    lbl.FontColor = (UInt32)(ColorTranslator.ToOle(Color.FromName("BLUE")));
                }
                else
                {
                    lbl.FontName = "Arial";
                    lbl.FontSize = 7;
                    lbl.FontColor = (UInt32)(ColorTranslator.ToOle(Color.Black));
                }

                lbl.FontBold = false;
                lbl.FontOutlineVisible = true;
                lbl.FontOutlineColor = (UInt32)(ColorTranslator.ToOle(Color.LightGray));
                lbl.FontOutlineWidth = 4;
                lbl.OffsetX = 2;
                lbl.OffsetY = 2;
                lbl.ShadowColor = (UInt32)(ColorTranslator.ToOle(Color.Gray));

                //MessageBox.Show("4");
                frmM.axMap.Redraw();
                frmM.Refresh();
                Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
        /// <summary>
        /// set label of shapefiles and its categories
        /// </summary>
        /// <param name="sf"></param>
        /// <param name="fldIndex"></param>
        /// <param name="FontName"></param>
        /// <param name="ccolor"></param>
        /// <param name="fontSize"></param>
        /// <param name="boolFrame"></param>
        /// <returns></returns>
        public Shapefile setlabel2(Shapefile sf, int fldIndex, string FontName, Color fontcolor, int fontSize, bool boolFrame, bool lblVisible, tkLabelFrameType frametype, uint bgcolor1, uint bgcolor2,bool remduplicates,bool avoidcoll)
        {
            
            int intLbl = -1;
            if (sf.ShapefileType == ShpfileType.SHP_POLYGON)
                intLbl = sf.GenerateLabels(fldIndex, MapWinGIS.tkLabelPositioning.lpInteriorPoint, true);
            else if(sf.ShapefileType == ShpfileType.SHP_POINT)
                intLbl = sf.GenerateLabels(fldIndex, tkLabelPositioning.lpCentroid,false);
            else
                intLbl = sf.GenerateLabels(fldIndex, tkLabelPositioning.lpLongestSegement,false);
                //generate labels (name field is expected in attribute table)

            sf.Labels.Synchronized = true;
            String strlayernm = String.Empty;

            for (int i = 0; i < frmM.legend1.Groups[frmM.m_groupvector].LayerCount; i++)
            {
                if (frmM.legend1.Layers.ItemByHandle(frmM.legend1.Groups[frmM.m_groupvector].LayerHandle(i)).Type != eLayerType.Image)
                {
                    Shapefile sf1 = (Shapefile)frmM.axMap.get_GetObject(frmM.legend1.Groups[frmM.m_groupvector].LayerHandle(i));
                    if (sf.Filename.ToUpper() == sf1.Filename.ToUpper())
                    {
                        strlayernm = frmM.legend1.Map.get_LayerName(i);
                        break;
                    }
                }
            }

            if (intLbl <= 0)
            {
                MessageBox.Show("No labels were generated, \n\rplease check the " + strlayernm + " layer field.", "Labelling Tool");
                return sf;
            }
                
            Labels lbl = sf.Labels;

            

            //setting font
            lbl.FontColor = Convert.ToUInt32(ColorTranslator.ToOle(fontcolor));
            lbl.FontName = FontName;
            lbl.FontSize = fontSize;
            lbl.FontOutlineVisible = false;
            lbl.ShadowColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Yellow));
            lbl.ShadowVisible = false;
            lbl.HaloColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Yellow));
            lbl.HaloVisible = false;
            lbl.HaloSize = 0;
            //lbl.TextRenderingHint = tkTextRenderingHint.ClearTypeGridFit;

            if (boolFrame == true)
            {
                //setting frame
                lbl.FrameVisible = boolFrame;
                //lbl.FrameType = tkLabelFrameType.lfRoundedRectangle;
                lbl.FrameType = frametype;
                lbl.FramePaddingY = 10;
                lbl.FramePaddingX = 10;
                lbl.FrameOutlineStyle = tkDashStyle.dsSolid;
                lbl.FrameOutlineColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray));
                lbl.FrameGradientMode = tkLinearGradientMode.gmHorizontal;
                lbl.FrameBackColor = bgcolor1;
                lbl.FrameBackColor2 = bgcolor2;
                lbl.FrameOutlineWidth = 1;
            }
            else
                lbl.FrameVisible = boolFrame;

            //
            //positioning
            lbl.VerticalPosition = tkVerticalPosition.vpAboveAllLayers;
            if (sf.ShapefileType != ShpfileType.SHP_POINT)
                lbl.Alignment = tkLabelAlignment.laCenter;
            else
                lbl.Alignment = tkLabelAlignment.laTopCenter;
            lbl.RemoveDuplicates = remduplicates;
            lbl.AvoidCollisions = avoidcoll;
            
            if (sf.ShapefileType == ShpfileType.SHP_POLYLINE)
                lbl.LineOrientation = tkLineLabelOrientation.lorParallel;
            else
                lbl.LineOrientation = tkLineLabelOrientation.lorHorizontal;

            lbl.UseGdiPlus = false;
            if (lblVisible)
                lbl.Visible = true;
            else
                lbl.Visible = false;

            for (int i = 0; i < sf.Labels.Count; i++)
            {
                if (lbl.get_Label(i, 0).Text.Contains("|") == true)
                {
                    MapWinGIS.Label lb = lbl.get_Label(i, 0);
                    string val = lb.Text;
                    string[] arr = val.Split('|');

                    StringBuilder sb = new StringBuilder();
                    foreach (string ar in arr)
                    {
                        string newval = ar.Replace(".", "");
                        if (newval != "")
                        {
                            sb.Append(ar);
                            sb.Append(Environment.NewLine);
                        }
                    }
                    lb.Text = sb.ToString();
                }
            }

            return sf;
        }

        public Shapefile setlabel3(Shapefile sf, int fldIndex, string FontName, Color fontcolor, int fontSize, bool boolFrame, bool lblVisible, tkLabelFrameType frametype, uint bgcolor1, uint bgcolor2, bool remduplicates, bool avoidcoll, bool wShadow, bool wHalo)
        {
            int intLbl = -1;
            if (sf.ShapefileType == ShpfileType.SHP_POLYGON)
                intLbl = sf.GenerateLabels(fldIndex, MapWinGIS.tkLabelPositioning.lpCenter, true);
            else if (sf.ShapefileType == ShpfileType.SHP_POINT)
                intLbl = sf.GenerateLabels(fldIndex, tkLabelPositioning.lpCenter, false);
            else
                intLbl = sf.GenerateLabels(fldIndex, tkLabelPositioning.lpMiddleSegment, false);
            //generate labels (name field is expected in attribute table)

            sf.Labels.Synchronized = true;
            if (intLbl <= 0)
            {
                MessageBox.Show("No labels were generated, \n\rplease check the barangay boundary layer field.", "Labelling Tool");
                return sf;
            }

            Labels lbl = sf.Labels;

            //setting font
            lbl.FontColor = Convert.ToUInt32(ColorTranslator.ToOle(fontcolor));
            lbl.FontName = FontName;
            lbl.FontSize = fontSize;
            lbl.FontOutlineVisible = false;
            if (wShadow == true)
            {
                lbl.ShadowColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray));
                lbl.ShadowVisible = true;
                lbl.ShadowOffsetX = 1;
                lbl.ShadowOffsetY = 1;
            }
            lbl.ShadowVisible = wShadow;
            if(wHalo==true)
            {
                lbl.HaloColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray));
                lbl.HaloVisible = true;
                lbl.HaloSize = 7;
                //lbl.TextRenderingHint = tkTextRenderingHint.ClearTypeGridFit;
            }
            lbl.HaloVisible = wHalo;
            if (boolFrame == true)
            {
                //setting frame
                lbl.FrameVisible = boolFrame;
                //lbl.FrameType = tkLabelFrameType.lfRoundedRectangle;
                lbl.FrameType = frametype;
                lbl.FramePaddingY = 10;
                lbl.FramePaddingX = 10;
                lbl.FrameOutlineStyle = tkDashStyle.dsSolid;
                lbl.FrameOutlineColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray));
                lbl.FrameGradientMode = tkLinearGradientMode.gmHorizontal;
                lbl.FrameBackColor = bgcolor1;
                lbl.FrameBackColor2 = bgcolor2;
                lbl.FrameOutlineWidth = 1;
            }
            lbl.FrameVisible = boolFrame;

            //positioning
            lbl.VerticalPosition = tkVerticalPosition.vpAboveAllLayers;
            lbl.Alignment = tkLabelAlignment.laCenter;
            lbl.RemoveDuplicates = remduplicates;
            lbl.AvoidCollisions = avoidcoll;
            //lbl.ScaleLabels = true;

            if (sf.ShapefileType == ShpfileType.SHP_POLYLINE)
                lbl.LineOrientation = tkLineLabelOrientation.lorParallel;
            else
                lbl.LineOrientation = tkLineLabelOrientation.lorHorizontal;

            
            lbl.MaxVisibleScale = 10000;
            lbl.MinVisibleScale = 0.0001;
            lbl.DynamicVisibility = true;

            lbl.UseGdiPlus = false;
            if (lblVisible)
                lbl.Visible = true;
            else
                lbl.Visible = false;


            return sf;
        }

        /// <summary>
        /// labelling for pt shapefile
        /// </summary>
        /// <param name="sf"></param>
        /// <param name="fldIndex"></param>
        /// <param name="FontName"></param>
        /// <param name="fontcolor"></param>
        /// <param name="fontSize"></param>
        /// <param name="boolFrame"></param>
        /// <param name="lblVisible"></param>
        /// <param name="frametype"></param>
        /// <param name="bgcolor1"></param>
        /// <param name="bgcolor2"></param>
        /// <param name="remduplicates"></param>
        /// <param name="avoidcoll"></param>
        /// <returns></returns>
        public Shapefile setptlabel(Shapefile sf, int fldIndex, string FontName, Color fontcolor, int fontSize, bool boolFrame, bool lblVisible, tkLabelFrameType frametype, uint bgcolor1, uint bgcolor2, bool remduplicates, bool avoidcoll, tkLabelAlignment labelalign, bool autoupdateoffset)
        {
            int intLbl = -1;
            if (sf.ShapefileType == ShpfileType.SHP_POLYGON)
                intLbl = sf.GenerateLabels(fldIndex, MapWinGIS.tkLabelPositioning.lpCenter, true);
            else if (sf.ShapefileType == ShpfileType.SHP_POINT)
                intLbl = sf.GenerateLabels(fldIndex, tkLabelPositioning.lpCentroid, false);
            else
                intLbl = sf.GenerateLabels(fldIndex, tkLabelPositioning.lpLongestSegement, false);
            //generate labels (name field is expected in attribute table)

            sf.Labels.Synchronized = true;
            if (intLbl <= 0)
            {
                MessageBox.Show("No labels were generated, \n\rplease check the barangay boundary layer field.", "Labelling Tool");
                return sf;
            }

            Labels lbl = sf.Labels;

            //setting font
            lbl.FontColor = Convert.ToUInt32(ColorTranslator.ToOle(fontcolor));
            lbl.FontName = FontName;
            lbl.FontSize = fontSize;
            lbl.FontOutlineVisible = false;
            lbl.ShadowColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Yellow));
            lbl.ShadowVisible = false;
            lbl.HaloColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Yellow));
            lbl.HaloVisible = false;
            lbl.HaloSize = 0;
            //lbl.TextRenderingHint = tkTextRenderingHint.ClearTypeGridFit;

            if (boolFrame == true)
            {
                //setting frame
                lbl.FrameVisible = true;
                //lbl.FrameType = tkLabelFrameType.lfRoundedRectangle;
                lbl.FrameType = frametype;
                lbl.FramePaddingY = 10;
                lbl.FramePaddingX = 10;
                lbl.FrameOutlineStyle = tkDashStyle.dsSolid;
                lbl.FrameOutlineColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray));
                lbl.FrameGradientMode = tkLinearGradientMode.gmHorizontal;
                lbl.FrameBackColor = bgcolor1;
                lbl.FrameBackColor2 = bgcolor2;
                lbl.FrameOutlineWidth = 1;
            }
            lbl.FrameVisible = boolFrame;

            //positioning
            lbl.VerticalPosition = tkVerticalPosition.vpAboveAllLayers;
            lbl.Alignment  = labelalign;
            lbl.AutoOffset = autoupdateoffset;
            lbl.RemoveDuplicates = remduplicates;
            lbl.AvoidCollisions = avoidcoll;

            if (sf.ShapefileType == ShpfileType.SHP_POLYLINE)
                lbl.LineOrientation = tkLineLabelOrientation.lorParallel;
            else
                lbl.LineOrientation = tkLineLabelOrientation.lorHorizontal;

            lbl.UseGdiPlus = false;
            if (lblVisible)
                lbl.Visible = true;
            else
                lbl.Visible = false;


            return sf;
        }

        public ArrayList populate(string strLayer, string strfld, string sBrgy)
        {
            ArrayList arrList = new ArrayList();
            Shapefile sf = new Shapefile();
            int lyr = -1;

            for (int i = 0; i < frmM.legend1.Layers.Count; i++)
            {
                if (frmM.legend1.Map.get_LayerName(i).ToUpper() == strLayer)
                {
                    sf = (Shapefile)frmM.axMap.get_GetObject(i);
                    lyr = i;
                    break;
                }
            }
            if (lyr != -1)
            {
                frmM.legend1.SelectedLayer = lyr;
                string fldName = strfld;

                int fldIdx = -1;
                fldIdx = sf.Table.get_FieldIndexByName(fldName);
                if (fldIdx != -1)
                {
                    sf.SelectNone();
                    for (int ii = 0; ii < sf.NumShapes; ii++)
                    {
                        if (sf.get_CellValue(fldIdx, ii).ToString().ToUpper().Trim() != String.Empty)
                        {
                            if (strLayer == "BARANGAY BOUNDARY")
                                arrList.Add(sf.get_CellValue(fldIdx, ii).ToString().ToUpper());
                            else if (strLayer == "SECTION BOUNDARY")
                            {
                                if (sf.get_CellValue(fldIdx, ii).ToString().ToUpper().Trim() == sBrgy)
                                {
                                    int fldIdx2  = -1;
                                    fldIdx2 = sf.Table.get_FieldIndexByName("SECT");
                                    arrList.Add(sf.get_CellValue(fldIdx2, ii).ToString().ToUpper().Trim());
                                }
                            }

                        }
                    }
                }
                else
                    MessageBox.Show("Can't find the " + fldName + " field name from the " + strLayer + " layer, \n\rplease check your map configuration.","Missing Field");
            }
            return arrList;
        }

        public ArrayList RemoveDups(ArrayList items)
        {

            ArrayList arrFinal = new ArrayList();

            foreach (object strItem in items)
            {
                
                if (!arrFinal.Contains(strItem.ToString().Trim()))
                {

                    arrFinal.Add(strItem.ToString().Trim());

                }

            }

            arrFinal.Sort();

            return arrFinal;

        }

        public static List<T> RemoveRepeatElement<T>(List<T> source)
        {
            Dictionary<T, int> listofUniqueElement = new Dictionary<T, int>();
            List<T> listofdest = new List<T>();

            foreach (T item in source)
            {
                if (!listofUniqueElement.ContainsKey(item))
                {
                    listofdest.Add(item);
                    listofUniqueElement.Add(item, 0);
                }
            }
            return listofdest;
        }

        public void zoomtoselected2(int intlayer, int intshp)
        {
            frmM.axMap.ZoomToShape(intlayer, intshp);
            FlashShape(3, intlayer, intshp);
        }

        public void zoomtoselected(string strlayername, string strfldname,string stritem, bool clearSelect)
        {
            try
            {
                //Shape sh = null;
                int lyr = -1;
                Shapefile sf = new Shapefile();
                for (int i = 0; i < frmM.legend1.Layers.Count; i++)
                {
                    if (frmM.legend1.Map.get_LayerName(i).ToUpper() == strlayername)
                    {
                        sf = (Shapefile)frmM.axMap.get_GetObject(i);
                        lyr = i;
                        break;
                    }
                }
                if (lyr != -1)
                {
                    frmM.legend1.SelectedLayer = lyr;
                    int fldIdx = -1;
                    fldIdx = sf.Table.get_FieldIndexByName(strfldname);

                    bool shapefound = false;
                    if (fldIdx != -1)
                    {
                        sf.SelectNone();
                        for (int ii = 0; ii < sf.NumShapes; ii++)
                        {
                            if (sf.get_CellValue(fldIdx, ii).ToString().ToUpper().Trim() == stritem.ToUpper())
                            {
                                sf.set_ShapeSelected(ii, true);
                                frmM.axMap.ZoomToShape(lyr, ii);
                                FlashShape(3, lyr, ii);
                                shapefound = true;
                                if(clearSelect==true)
                                    sf.SelectNone();//remarked april 25 2013
                                break;
                            }
                        }
                        if (shapefound == false)
                        {
                            MessageBox.Show("Can't locate the searched shape from the map.","Selection");
                            frmM.legend1.Refresh();
                            frmM.axMap.Redraw();
                        }
                        frmM.axMap.Invalidate();
                        frmM.axMap.Refresh();
                        frmM.axMap.Redraw();
                    }
                }
                else
                {
                    MessageBox.Show("Can't locate the searched shape from the map.", "Selection");
                    //MessageBox.Show("Barangay name can't be found from the list in barangay boundary layer.", "Confirm");
                }
                //return sh;
            }
            catch
            {
                //return sh;
            }

        }

        /// <summary>
        /// get the selected shape and returns the int of the shapes as its shapeid
        /// return -1 if none were selected
        /// </summary>
        /// <param name="xCoord"></param>
        /// <param name="yCoord"></param>
        /// <param name="sf"></param>
        /// <returns></returns>
        public int getSelectedShape(double xCoord, double yCoord, MapWinGIS.Shapefile sf)
        {
            try
            {
                MapWinGIS.Extents boundBox = new MapWinGIS.Extents();
                boundBox.SetBounds(xCoord - 0, yCoord - 0, 0, xCoord + 0, yCoord + 0, 0);
                object selectedShapes = new object();
                if (!sf.SelectShapes(boundBox, 0, MapWinGIS.SelectMode.INTERSECTION, ref selectedShapes))
                    return -1;
                int[] shapes = (int[])selectedShapes;
                //Clean up:
                boundBox = null;
                return shapes[0];
            }
            catch
            {
                return -1;
                //throw new Exception("Error in getting the selected shape: \n" + ex.ToString());
                
            }

        }

        public void setclickdefault()
        {
            frmM.axMap.CursorMode = tkCursorMode.cmNone;
            frmM.axMap.MapCursor = tkCursor.crsrMapDefault;
            frmM.axMap.SendMouseDown = false;

            foreach (ToolStripButton item in frmM.mutuallyExclusiveButtons)
            {
                item.Checked = false;
            }
        }


        /// <summary>
        /// opening parcel shapefiles dynamically
        /// </summary>
        /// <param name="strbrgynm"></param>
        /// <param name="strbrgyno"></param>
        public Shapefile loadparcel(string strbrgynm,string strbrgyno)
        {
            string strno = String.Empty;
            if (strbrgyno.Length == 3)
                strno = strbrgyno.Substring(1, 2);
            Shapefile sf = new MapWinGIS.Shapefile();
            if (sf != null)
            {
                sf.Open(frmM.m_shapelandlocation + @"\" + "d" + strno + ".shp", null);
                string strLayerName = "BARANGAY " + strbrgynm.ToUpper();
                string strfldname = String.Empty;//"PIN";
                int iFldInx = -1;
                Color cfontcolor = Color.White;
                int intfontsize = 4;

                string sLayerName = "LAND PARCEL";
                string sqry = String.Format("[LAYER_NAME] = '{0}'", sLayerName);
                DataRow[] foundRows = frmM.m_dtlayer.Select(sqry);
                if (foundRows.Length > 0)
                {
                    foreach (DataRow dr in foundRows)
                    {
                        strfldname = dr["LABEL"].ToString().Trim();
                    }
                }

                if (sf.Table != null)
                {
                    iFldInx = sf.Table.get_FieldIndexByName(strfldname);

                    if (iFldInx == -1)
                    {
                        MessageBox.Show("Please check the " + strLayerName + " layer, the barangay name field is missing.", "Confirm");
                    }
                    else
                    {
                        //setting the categories for unique values and gradient legend
                        //sf = uniquecategories3(sf, strfldname, strLayerName);
                        //loadShapeFileBoundary(@"R:\AparriGIS\Themes\land\d" + strno + ".shp", strLayerName, false, Convert.ToUInt32(ColorTranslator.ToOle(Color.Green)), Convert.ToUInt32(ColorTranslator.ToOle(Color.Green)), (float)2, true, strfldname);
                        sf = legendPolygonSingle(sf, Convert.ToUInt32(ColorTranslator.ToOle(Color.Lime)), (float)1, false, (UInt32)(ColorTranslator.ToOle(Color.Transparent)));
                        //setting the labels
                        checklabelregistry();
                        if (frmM.m_shapelandlabelset == true)
                        {
                            //CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                            //TextInfo textInfo = cultureInfo.TextInfo;
                            //sf = setLabelRegistry(sf, textInfo.ToTitleCase("land parcel");
                            ////clsLabelProp clslp = new clsLabelProp();
                            ////clslp.LayerName = "";
                            //string labelfield = String.Empty;
                            //string labelfont = String.Empty;
                            //string fontcolor = String.Empty;
                            //int ifontsize = -1;
                            //bool framevisible;
                            //bool fvisible;
                            //bool labelvisible;
                            //bool lvisible;
                            //tkLabelFrameType frametype = tkLabelFrameType.lfPointedRectangle;
                            //string framecolorleft = String.Empty;
                            //string framecolorright = String.Empty;
                            //bool removeduplicates;
                            //bool rdupli;
                            //bool avoidcollision;
                            //bool acollision;

                            //clsRegistry clsreg = new clsRegistry();
                            //string strhive = "CurrentUser";
                            //string straddress = @"Software\Amellar\PARCEL.NET\Layers\Land Parcel\Label";
                            //Microsoft.Win32.RegistryKey baca = clsreg.Hive(strhive);
                            //Microsoft.Win32.RegistryKey baca1 = baca.OpenSubKey(straddress);
                            //if (baca1 == null)
                            //    baca1 = baca.CreateSubKey(straddress);

                            //baca1 = baca.OpenSubKey(straddress);
                            //labelfield = clsreg.ReadDword(strhive, straddress, "LabelField");
                            //labelfont = clsreg.ReadDword(strhive, straddress, "LabelFont").ToUpper();
                            //if (labelfont == "")
                            //    labelfont = "ARIAL";
                            //fontcolor = clsreg.ReadDword(strhive,straddress,"FontColor");
                            //if (fontcolor == "")
                            //    fontcolor = "White";
                            //if(Int32.TryParse(clsreg.ReadDword(strhive,straddress,"LabelFontSize"), out ifontsize))
                            //    intfontsize = ifontsize;//Convert.ToInt32(clsreg.ReadDword(strhive, straddress, "LabelFontSize"));
                            //if (Boolean.TryParse(clsreg.ReadDword(strhive, straddress, "FrameVisible"), out fvisible))
                            //    framevisible = fvisible;
                            //else
                            //    framevisible = false;
                            //if (Boolean.TryParse(clsreg.ReadDword(strhive, straddress, "FrameVisible"), out lvisible))
                            //    labelvisible = lvisible;
                            //else
                            //    labelvisible = false;

                            //if (clsreg.ReadDword(strhive, straddress, "FrameType").ToUpper() == "lfPointedRectangle".ToUpper())
                            //    frametype = tkLabelFrameType.lfPointedRectangle;
                            //else if (clsreg.ReadDword(strhive, straddress, "FrameType").ToUpper() == "lfRectangle".ToUpper())
                            //    frametype = tkLabelFrameType.lfRectangle;
                            //else if (clsreg.ReadDword(strhive, straddress, "FrameType").ToUpper() == "lfRoundedRectangle".ToUpper())
                            //    frametype = tkLabelFrameType.lfRoundedRectangle;

                            //framecolorleft = clsreg.ReadDword(strhive, straddress, "FrameColorLeft");
                            //if (framecolorleft == "")
                            //    framecolorleft = "DarkGreen";

                            //framecolorright = clsreg.ReadDword(strhive,straddress,"FrameColorRight");
                            //if (framecolorright == "")
                            //    framecolorright = "PaleGreen";

                            //if (Boolean.TryParse(clsreg.ReadDword(strhive, straddress, "RemoveDuplicates"), out rdupli))
                            //    removeduplicates = rdupli;
                            //else
                            //    removeduplicates = false;

                            //if (Boolean.TryParse(clsreg.ReadDword(strhive, straddress, "AvoidCollision"), out acollision))
                            //    avoidcollision = acollision;
                            //else
                            //    avoidcollision = false;

                            //int ifldidx = sf.Table.get_FieldIndexByName(labelfield);
                            //if(ifldidx != -1)
                            //    sf = setlabel2(sf, ifldidx, labelfont, Color.FromName(fontcolor), intfontsize, framevisible, labelvisible, frametype, Convert.ToUInt32(ColorTranslator.ToOle(Color.FromName(framecolorleft))), Convert.ToUInt32(ColorTranslator.ToOle(Color.FromName(framecolorright))), removeduplicates, avoidcollision);
                        }
                        else
                            sf = setlabel2(sf, iFldInx, "ARIAL", cfontcolor, intfontsize, true, false, tkLabelFrameType.lfRectangle, Convert.ToUInt32(ColorTranslator.ToOle(Color.DarkGreen)), Convert.ToUInt32(ColorTranslator.ToOle(Color.PaleGreen)), false, false);

                        //adding the shapefile as layer
                        frmM.hndMap = frmM.legend1.Layers.Add(sf, true);
                        frmM.legend1.Layers.MoveLayer(frmM.hndMap, frmM.m_groupvector, frmM.legend1.Groups[frmM.m_groupvector].LayerCount);
                        frmM.legend1.Map.set_LayerName(frmM.hndMap, strLayerName);
                        frmM.legend1.Layers.ItemByHandle(frmM.hndMap).Expanded = false;
                        frmM.legend1.Layers.ItemByHandle(frmM.hndMap).Refresh();
                    }

                    frmM.hndMapSel = frmM.hndMap;
                    frmM.m_sellayername = strLayerName;
                    frmM.legend1.Map.Redraw();
                }
            }
            return sf;
        }

        /// <summary>
        /// return the shapefile with legend as line boundary only
        /// </summary>
        /// <param name="sf"></param>
        /// <param name="strLayerName"></param>
        /// <param name="lineColor"></param>
        /// <param name="lineWidth"></param>
        /// <param name="isZoomExt"></param>
        /// <returns></returns>
        public MapWinGIS.Shapefile legendPolyBoundaryOnly(MapWinGIS.Shapefile sf, UInt32 lineColor, float lineWidth)
        {
            try
            {
                //for setting the fill values
                sf.DefaultDrawingOptions.FillTransparency = (float)0;
                sf.DefaultDrawingOptions.FillType = tkFillType.ftStandard;
                sf.DefaultDrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Transparent));
                sf.DefaultDrawingOptions.FillBgTransparent = true;
                //for setting the outline values
                sf.DefaultDrawingOptions.LineColor = lineColor;
                sf.DefaultDrawingOptions.LineTransparency = (float)255;
                sf.DefaultDrawingOptions.LineWidth = lineWidth;
                //for clearing the category
                sf.Categories.Clear();
                //for setting the visibility
                sf.DefaultDrawingOptions.FillVisible = false;
                sf.DefaultDrawingOptions.LineVisible = true;
                sf.DefaultDrawingOptions.VerticesVisible = false;
                return sf;
            }
            catch
            {
                sf = null;
                return sf;
            }
        }

        public Shapefile legendPolygonSingle(Shapefile sf, UInt32 lineColor, float lineWidth, bool isFVisible, UInt32 fillColor)
        {
            try
            {
                //for setting the fill values

                sf.DefaultDrawingOptions.FillType = tkFillType.ftStandard;
                if (isFVisible == true)
                {
                    sf.DefaultDrawingOptions.FillTransparency = (float)255;
                    sf.DefaultDrawingOptions.FillColor = fillColor;
                    sf.DefaultDrawingOptions.FillBgTransparent = false;
                }
                else
                {
                    sf.DefaultDrawingOptions.FillTransparency = (float)0;
                    sf.DefaultDrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Transparent));
                    sf.DefaultDrawingOptions.FillBgTransparent = true;
                }

                //for setting the outline values
                sf.DefaultDrawingOptions.LineColor = lineColor;
                sf.DefaultDrawingOptions.LineTransparency = (float)255;
                sf.DefaultDrawingOptions.LineWidth = lineWidth;
                //for clearing the category
                sf.Categories.Clear();

                //for setting the visibility
                if (isFVisible == true)
                    sf.DefaultDrawingOptions.FillVisible = true;
                else
                    sf.DefaultDrawingOptions.FillVisible = false;
                sf.DefaultDrawingOptions.LineVisible = true;
                sf.DefaultDrawingOptions.VerticesVisible = false;
                return sf;
            }
            catch
            {
                sf = null;
                return sf;
            }
        }

        public Shapefile legendPointSingle(Shapefile sf, UInt32 trnsColor, UInt32 trnsColor2, bool useTrans, double picXscale, double picYscale)
        {
            try
            {
                Utils myUtil = new Utils();
                //new image to be the icon representation of the point
                MapWinGIS.Image img = new MapWinGIS.Image();
                
                img.TransparencyColor = trnsColor;//Convert.ToUInt32(ColorTranslator.ToOle(Color.LightGray));
                img.TransparencyColor2 = trnsColor2;//Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray));

                if (useTrans)
                    img.UseTransparencyColor = true;
                else
                    img.UseTransparencyColor = false;

                //the image can be loaded from a location or can be retreived from the resources
                //bool boolOK = img.Open(@"D:\mapwindow\AmellarGISParcelMapping\AmellarGISParcelMapping\Resources\zoomextentblue.png", ImageType.USE_FILE_EXTENSION,false, null);

                if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\building\bldgpt.png"))
                {
                    img.Open(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\building\bldgpt.png", ImageType.USE_FILE_EXTENSION, false, null);
                    sf.DefaultDrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                    sf.DefaultDrawingOptions.Picture = img;
                }
                else
                    sf.DefaultDrawingOptions.PointShape = tkPointShapeType.ptShapeCircle;

                //img.Picture = myUtil.hBitmapToPicture(Properties.Resources.homeblue241.GetHbitmap().ToInt32());//IconConverter.GetIPictureDispFromImage(Properties.Resources.homeblue241);

                //setting the default values of the point feature
                //sf.DefaultDrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                //sf.DefaultDrawingOptions.Picture = img;
                //sf.DefaultDrawingOptions.Picture = img;// myUtil.hBitmapToPicture(Properties.Resources.homeblue241.GetHbitmap().ToInt32());
                sf.DefaultDrawingOptions.PictureScaleX = picXscale;//1
                sf.DefaultDrawingOptions.PictureScaleY = picYscale;//1
                sf.CollisionMode = tkCollisionMode.AllowCollisions;
                sf.DefaultDrawingOptions.PointRotation = (double)0;
                sf.DefaultDrawingOptions.PointSidesRatio = (float)0.5;
                
                return sf;
            }
            catch
            {
                sf = null;
                return sf;
            }
        }

        public Shapefile legendLineSingle(Shapefile sf, UInt32 lineColor, float lineWidth, bool isVisible)
        {
            try
            {
                //for setting the outline values
                if (sf.ShapefileType == ShpfileType.SHP_POLYLINE)
                {
                    sf.DefaultDrawingOptions.LineColor = lineColor;
                    sf.DefaultDrawingOptions.LineWidth = lineWidth;
                    if (isVisible == true)
                        sf.DefaultDrawingOptions.LineTransparency = (float)255;
                    else
                        sf.DefaultDrawingOptions.LineTransparency = (float)0;
                    //for clearing the category
                    sf.Categories.Clear();

                    //for setting the visibility
                    if (isVisible == true)
                        sf.DefaultDrawingOptions.LineVisible = true;
                    else
                        sf.DefaultDrawingOptions.LineVisible = false;

                    sf.DefaultDrawingOptions.VerticesVisible = false;
                    return sf;
                }
                else
                    sf = null;
                return sf;
            }
            catch
            {
                sf = null;
                return sf;
            }
        }

        public bool colorByUniqueBreaks(int fld, int lyrNum)
        {
            ShapefileColorScheme coloringscheme = new ShapefileColorSchemeClass();
            Hashtable ht = new Hashtable();
            Shapefile sf = (Shapefile)frmM.axMap.get_GetObject(lyrNum);
            object val;

            if (sf == null)
            {
                return false;
            }

            if ((fld > (sf.NumFields - 1)) || (lyrNum < 0))
            {
                return false;
            }

            do
            {
                coloringscheme.Remove(0);
            } while (coloringscheme.NumBreaks() > 0);

            for (int i = 0; i < sf.NumShapes; i++)
            {
                val = sf.get_CellValue(fld, i);
                if (ht.ContainsKey(val) == false)
                    ht.Add(val, val);
            }
            string[] arr;
            arr = new String[ht.Count];
            ht.Values.CopyTo(arr, 0);
            Array.Sort(arr);

            string strcolor = String.Empty;
            for (int ii = 0; ii < arr.Length; ii++)
            {
                ShapefileColorBreak brk = new ShapefileColorBreakClass();
                Hashtable usedColors = new Hashtable();

                switch (arr[ii].ToString().ToUpper())
                {
                    case "RESIDENTIAL":
                        strcolor = "YELLOW";
                        break;
                    case "AGRICULTURAL":
                        strcolor = "GREEN";
                        break;
                    case "COMMERCIAL":
                        strcolor = "RED";
                        break;
                    case "INDUATRIAL":
                        strcolor = "VIOLET";
                        break;
                    case "MINERAL":
                        strcolor = "BROWN";
                        break;
                    case "SPACIAL":
                        strcolor = "TAN";
                        break;
                    default:
                        strcolor = "TRANSPARENT";
                        break;
                }
                Color ccolor = Color.FromName(strcolor);
                if (ccolor != null)
                {
                    UInt32 randomColor = (UInt32)System.Drawing.ColorTranslator.ToOle(ccolor);
                    brk.StartColor = randomColor;
                    brk.EndColor = randomColor;
                    brk.StartValue = arr[ii] as object;
                    brk.EndValue = arr[ii] as object;

                    if (IsNumeric(arr[ii].ToString()))
                        brk.Caption = (Convert.ToDouble(arr[ii].ToString())).ToString("G3");
                    else
                        brk.Caption = (arr[ii]).ToString();

                    coloringscheme.FieldIndex = fld;
                    coloringscheme.Add(brk);
                    brk = null;
                    usedColors = null;
                }
            }
            ht = null;
            sf = null;

            //'Attach colorscheme to layer:
            coloringscheme.LayerHandle = frmM.legend1.Layers[lyrNum].Handle;
            frmM.axMap.ApplyLegendColors(coloringscheme);

            return true;
        }

        public static bool IsNumeric(string input)
        {
            try
            {
                if (input != null)
                {
                    double output;
                    return Double.TryParse(Convert.ToString(input), out output);
                }
                else
                    return false;
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
                return false;
            }
        }

        public void initServerDate()
        {
            DateTime dt = getDBServerDate();
            c_serverdate = String.Format("{0:d-MMM-yyyy}", dt);
            c_currentyear = String.Format("{0:yyyy}", dt);
        }

        public DateTime getDBServerDate()
        {
            OracleResultSet resultSet = new OracleResultSet();
            DateTime dtresult = DateTime.Now;
            resultSet.Query = "select SYSDATE from dual";
            if (resultSet.Execute())
            {
                while (resultSet.Read())
                {
                    dtresult = resultSet.GetDateTime("SYSDATE");
                }
            }
            return dtresult;
        }

        public string DBServerDate(string dsnOLEDB)
        {
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                String sConnString = dsnOLEDB;
                OleDbCommand cmd = new OleDbCommand();
                OleDbConnection oOleDbConnection = new OleDbConnection(frmM.m_strconn);
                oOleDbConnection.Open();

                String squery = "select SYSDATE CURRENTSERVERDATE from dual";

                cmd = new OleDbCommand(squery, oOleDbConnection);
                OleDbDataAdapter newDataAdapter = new OleDbDataAdapter(cmd);
                DataSet newDataSet = new DataSet();
                OleDbDataReader newDataReaders = cmd.ExecuteReader();

                if (newDataReaders.HasRows)
                {
                    String strVal = "";
                    while (newDataReaders.Read())
                    {
                        if (!DBNull.Value.Equals(newDataReaders["CURRENTSERVERDATE"]))
                        {
                            strVal = (String)newDataReaders["CURRENTSERVERDATE"].ToString();
                        }
                    }
                    newDataReaders.Close();
                    oOleDbConnection.Close();
                    return strVal;
                }
                else
                {
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                    return (String)"";
                }

            }
            catch (Exception e)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                MessageBox.Show(e.Message);
                return (String)"";
            }
        }

        public bool layerExist(string lyrnm)
        {
            bool isExisting = false;
            for (int i = 0; i < frmM.legend1.Layers.Count; i++)
            {
                if (frmM.legend1.Map.get_LayerName(i).ToUpper() == lyrnm.ToUpper())
                {
                    isExisting = true;
                    break;
                }
            }
            return isExisting;
        }

        public void FlashShape(int nCyclesCnt, int nLayerHandle, int nShapeIdx)
        {
            try
            {
                //get the shapefile object of the layer handle. we assume its always a shapefile
                Shapefile sfl = (Shapefile)frmM.legend1.Map.get_GetObject(nLayerHandle);
                //now get the shape in the object.
                MapWinGIS.Shape shp = sfl.get_Shape(nShapeIdx);
                //get the center of the shape
                //http://www.mapwindow.org/wiki/index.php/MapWinGeoProc:Utils_Centroid


                MapWinGIS.Point pt = new MapWinGIS.Point();
                if (shp.ShapeType == ShpfileType.SHP_POLYGON)
                    pt = shp.Center;
                else if (shp.ShapeType == ShpfileType.SHP_POLYLINE)
                    pt = shp.Center;
                else if (shp.ShapeType == ShpfileType.SHP_POINT)
                    pt = shp.Center;

                //get the extents of the map
                Extents ext = (Extents)frmM.legend1.Map.Extents;

                //now draw two fat lines, one vertical and one horizontal to focus the center of the polygon
                int hndLineDrawing = frmM.legend1.Map.NewDrawing(tkDrawReferenceList.dlSpatiallyReferencedList);
                //// each line is extended 5 degrees to each side of the center of the polygon
                //mapMain.DrawLine(pt.x, pt.y - 5, pt.x, pt.y + 5, 2, ColorToUInteger(Color.Cyan)) 'horizontal
                //mapMain.DrawLine(pt.x - 5, pt.y, pt.x + 5, pt.y, 2, ColorToUInteger(Color.Cyan)) 'vertical
                ///// or extend the line from each side of the map extents to center of the polygon
                frmM.legend1.Map.DrawLine(pt.x, ext.yMin, pt.x, ext.yMax, 4, (UInt32)(ColorTranslator.ToOle(Color.Cyan))); //horizontal
                frmM.legend1.Map.DrawLine(ext.xMin, pt.y, ext.xMax, pt.y, 4, (UInt32)(ColorTranslator.ToOle(Color.Cyan))); //vertical

                //flash for a couple cycles, each one being 100 milliseconds in length
                //Dim i As Integer
                for (int i = 1; i < nCyclesCnt; i++)
                {
                    frmM.legend1.Map.set_ShapeVisible(nLayerHandle, nShapeIdx, false);
                    frmM.legend1.Refresh();
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(50);
                    Application.DoEvents();
                    frmM.legend1.Map.set_ShapeVisible(nLayerHandle, nShapeIdx, true);
                    frmM.legend1.Refresh();
                }

                //'and finally clear the line drawings
                frmM.legend1.Map.ClearDrawing(hndLineDrawing);
            }
            finally
            {
                frmM.legend1.Refresh();
            }
        }

        /// <summary>
        /// loading building shapefiles dynamically
        /// </summary>
        /// <param name="strbrgynm"></param>
        /// <param name="strbrgyno"></param>
        public Shapefile loadbldgptedited(string strbrgynm, string strbrgyno, string dsnOLEDB)
        {
            try
            {
                string strno = String.Empty;
                if (strbrgyno.Length == 3)
                    strno = strbrgyno.Substring(1, 2);

                //new shapefile to be created
                Shapefile sf = new MapWinGIS.Shapefile();
                if (System.IO.File.Exists("b" + strno + "_pt.shp"))
                    System.IO.File.Delete("b" + strno + "_pt.shp");
                if (System.IO.File.Exists("b" + strno + "_pt.dbf"))
                    System.IO.File.Delete("b" + strno + "_pt.dbf");
                if (System.IO.File.Exists("b" + strno + "_pt.shx"))
                    System.IO.File.Delete("b" + strno + "_pt.shx");
                if (System.IO.File.Exists("b" + strno + "_pt.prj"))
                    System.IO.File.Delete("b" + strno + "_pt.prj");
                
                bool result = sf.CreateNew("b" + strno + "_pt.shp", MapWinGIS.ShpfileType.SHP_POINT);

                int intfld = 0;
                if (result == true)
                    result = sf.StartEditingShapes(true, null);
                //create fieldnames
                MapWinGIS.Field fld = new MapWinGIS.FieldClass();
                fld.Name = "BLDG_PIN";
                fld.Width = 23;
                fld.Type = MapWinGIS.FieldType.STRING_FIELD;
                sf.EditInsertField(fld, ref intfld, null);

                intfld += 1;
                fld = new MapWinGIS.FieldClass();
                fld.Name = "LAND_PIN";
                fld.Width = 18;
                fld.Type = MapWinGIS.FieldType.STRING_FIELD;
                sf.EditInsertField(fld, ref intfld, null);

                intfld += 1;
                fld = new MapWinGIS.FieldClass();
                fld.Name = "BLDG_CNT";
                fld.Width = 6;
                fld.Type = MapWinGIS.FieldType.INTEGER_FIELD;
                sf.EditInsertField(fld, ref intfld, null);
                
                intfld += 1;
                fld = new MapWinGIS.FieldClass();
                fld.Name = "X";
                fld.Width = 10;
                fld.Precision = 6;
                fld.Type = MapWinGIS.FieldType.DOUBLE_FIELD;
                sf.EditInsertField(fld, ref intfld, null);

                intfld += 1;
                fld = new MapWinGIS.FieldClass();
                fld.Name = "Y";
                fld.Width = 10;
                fld.Precision = 6;
                fld.Type = MapWinGIS.FieldType.DOUBLE_FIELD;
                sf.EditInsertField(fld, ref intfld, null);

                //here we need to create a shapefile that would be based on the number of
                //declared improvements from the rpta database
                //but first we would need the list of land pins with improvements within the barangay
                //ArrayList bldgList = populateBrgyBldg(dsnOLEDB, strbrgyno, 0);//ver 1
                ArrayList bldgList = populateBrgyBldgII(dsnOLEDB, strbrgyno, 0);//ver 2
                //ArrayList bldgcntlist = populateBrgyBldg(dsnOLEDB, strbrgyno, 1);//ver 1
                if (bldgList.Count == 0)
                {
                    MessageBox.Show("No declared building were found for the selected barangay.");
                    return null;
                }
                else
                {
                    Shapefile landsf = null;
                    string brgylayer = String.Empty;
                    for (int i = 0; i < frmM.legend1.Layers.Count; i++)
                    {
                        if (frmM.legend1.Map.get_LayerName(i).ToUpper() == "BARANGAY " + strbrgynm.ToUpper())
                        {
                            landsf = (Shapefile)frmM.axMap.get_GetObject(i);
                            break;
                        }
                    }

                    if (landsf != null)
                    {
                        //before loading the building point: you need
                        //the land parcel boundary of the selected barangay
                        //where we will get its tied land parcel(centroids) 
                        string strLayerName = "BLDG: " + strbrgynm.ToUpper();
                        string strfldname = "PIN";
                        int iFldInx = -1;
                        Color cfontcolor = Color.White;
                        //int intfontsize = 4;
                        iFldInx = landsf.Table.get_FieldIndexByName(strfldname);

                        if (iFldInx == -1)
                        {
                            MessageBox.Show("Please check the " + strLayerName + " layer, the barangay name field is missing.", "Confirm");
                        }
                        else
                        {
                            bool boolSuccess = false;
                            string pinval = String.Empty;
                            int ptref = 0;
                            //version 1:
                            //add only 1 pt and uses the land shape centroid
                            //for (int iii = 0; iii < landsf.NumShapes; iii++)
                            //{
                            //    pinval = landsf.get_CellValue(iFldInx, iii).ToString();

                            //    for (int ii = 0; ii < bldgList.Count; ii++)
                            //    {
                            //        if (bldgList[ii].ToString() == pinval)
                            //        {
                            //            Shape sh = landsf.get_Shape(iii);
                            //            MapWinGIS.Point shpt = sh.Centroid;

                            //            Shape sh1 = new MapWinGIS.ShapeClass();
                            //            boolSuccess = sh1.Create(MapWinGIS.ShpfileType.SHP_POINT);
                            //            ptref = ptref + 1;
                            //            //if (boolSuccess)
                            //                boolSuccess = sh1.InsertPoint(shpt, ref ptref);
                            //            //if (boolSuccess)
                            //                boolSuccess = sf.EditInsertShape(sh1, ref ptref);
                            //            //if (boolSuccess)
                            //            //{
                            //                bool boolCellAdd;
                            //                boolCellAdd = sf.EditCellValue(0, ptref, bldgList[ii]);
                            //                boolCellAdd = sf.EditCellValue(1, ptref, bldgcntlist[ii]);
                            //            //}

                            //            break;
                            //        }
                            //    }
                            //}

                            //version 2:
                            //add random pt and uses the land shape as its boundaries
                            string lastpin = String.Empty;
                     
                            for (int ii = 0; ii < bldgList.Count; ii++)
                            {
                                int cntr = 1;
                                //for (int iii = 0; iii < landsf.NumShapes; iii++)remarked
                                //{remarked
                                    //pinval = landsf.get_CellValue(iFldInx, iii).ToString();//remarked
                                    pinval = bldgList[ii].ToString().Substring(0, 18);//added
                                    string error = "";//added may 15 2013
                                    MapWinGIS.Shape sh = new MapWinGIS.Shape();
                                    object newresult = null;//added may 15 2013
                                    int[] shapes2 = null;//added may 15 2013
                                    if (landsf.Table.Query("[PIN] = \"" + bldgList[ii].ToString().Substring(0, 18) + "\"", ref newresult, ref error))//added may 15 2013
                                    {//added may 15 2013//added may 15 2013

                                        shapes2 = newresult as int[];//added may 15 2013
                                        if (shapes2 != null)//added may 15 2013
                                        {//added may 15 2013
                                            sh = landsf.get_Shape(Convert.ToInt32(shapes2.GetValue(0).ToString()));
                                            //if (lastpin == "018-07-005-004-106")
                                            //    MessageBox.Show("aw");
                                        }//added may 15 2013


                                        //if (bldgList[ii].ToString().Substring(0,18) == pinval)remarked
                                        //{   remarked
                                        //sh = landsf.get_Shape(iii);remarked
                                        //MapWinGIS.Point shpt = sh.Center;//remarked

                                        MapWinGIS.Point qpt = sh.InteriorPoint;//landsf.QuickPoint(Convert.ToInt32(shapes2.GetValue(0).ToString()), 0);//added
                                        int cnt = 0;//added
                                        if (pinval == lastpin)
                                        {
                                            cntr += 1;
                                            //Random rnd = new Random(DateTime.Now.Millisecond);//remarked//remarked
                                            //shpt.x = sh.Extents.xMin + (sh.Extents.xMax - sh.Extents.xMin) * (rnd.NextDouble());//remarked
                                            //shpt.y = sh.Extents.yMin + (sh.Extents.yMax - sh.Extents.yMin) * (rnd.NextDouble());//remarked
                                            //new Coordinate(XMIN + rnd.NextDouble() * XSPAN, YMIN + rnd.NextDouble() * YSPAN);

                                            //double radius = rnd.Next() * 90;
                                            //shpt.x = shpt.x + radius * Math.Cos(Math.PI / 18);
                                            //shpt.y = shpt.y + radius * Math.Sin(Math.PI / 18);
                                            do//added
                                            {//added
                                                cnt += 1;//added
                                                qpt = qpt = sh.InteriorPoint;//landsf.QuickPoint(Convert.ToInt32(shapes2.GetValue(0).ToString()), cnt);//added//added
                                            } while (landsf.PointInShape(Convert.ToInt32(shapes2.GetValue(0).ToString()), qpt.x, qpt.y) == false);//added
                                        }

                                        //lastpin = pinval;//remarked
                                        MapWinGIS.Shape sh1 = new MapWinGIS.ShapeClass();
                                        boolSuccess = sh1.Create(MapWinGIS.ShpfileType.SHP_POINT);
                                        ptref = ptref + 1;
                                        //if (boolSuccess)
                                        //if(MapWinGeoProc.Utils.PointInPolygon(ref sh,ref shpt)==true)
                                        ////boolSuccess = sh1.InsertPoint(shpt, ref ptref);//remarked may 15 2013
                                        //start edit

                                        do//added
                                        {//added
                                            cnt += 1;//added
                                            qpt = sh.InteriorPoint;//landsf.QuickPoint(Convert.ToInt32(shapes2.GetValue(0).ToString()), cnt);//added
                                        } while (landsf.PointInShape(Convert.ToInt32(shapes2.GetValue(0).ToString()), qpt.x, qpt.y) == false);//added
                                        //if (boolSuccess)
                                        //bool testbool = landsf.PointInShape(iii, qpt.x, qpt.y);//added
                                        boolSuccess = sh1.InsertPoint(qpt, ref ptref);//added

                                        ////end edit
                                        boolSuccess = sf.EditInsertShape(sh1, ref ptref);
                                        //if (boolSuccess)
                                        //{
                                        bool boolCellAdd;
                                        boolCellAdd = sf.EditCellValue(0, ptref, bldgList[ii]);
                                        boolCellAdd = sf.EditCellValue(1, ptref, bldgList[ii].ToString().Substring(0, 18));
                                        boolCellAdd = sf.EditCellValue(3, ptref, qpt.x);
                                        boolCellAdd = sf.EditCellValue(4, ptref, qpt.y);
                                        //boolCellAdd = sf.EditCellValue(1, ptref, bldgcntlist[ii]);//remarked for ver 2
                                        //}

                                        lastpin = bldgList[ii].ToString().Substring(0, 18);//added
                                        //break;
                                    }
                                    else
                                        continue;
                                //}
                            }

                            boolSuccess = sf.StopEditingShapes(true, true, null);
                            sf.SaveAs(frmM.m_shapebldglocation + @"\" + sf.Filename, null);

                            sf.Open(sf.Filename, null);

                            //setting the categories for unique values and gradient legend
                            sf = legendPointSingle(sf, Convert.ToUInt32(ColorTranslator.ToOle(Color.LightGray)), Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray)), true, (double)1, (double)1);
                            //setting the labels
                            //sf = setlabel2(sf, iFldInx, "ARIAL", cfontcolor, intfontsize, true, false, tkLabelFrameType.lfRectangle, Convert.ToUInt32(ColorTranslator.ToOle(Color.DarkGreen)), Convert.ToUInt32(ColorTranslator.ToOle(Color.PaleGreen)), false, false);

                            //adding the shapefile as layer
                            frmM.hndMap = frmM.legend1.Layers.Add(sf, true);
                            frmM.legend1.Layers.MoveLayer(frmM.hndMap, frmM.m_groupvector, frmM.legend1.Groups[frmM.m_groupvector].LayerCount);
                            frmM.legend1.Map.set_LayerName(frmM.hndMap, strLayerName);
                            frmM.legend1.Layers.ItemByHandle(frmM.hndMap).Expanded = false;
                            frmM.legend1.Layers.ItemByHandle(frmM.hndMap).Refresh();
                        }

                        frmM.hndMapSel = frmM.hndMap;
                        frmM.m_sellayername = strLayerName;
                        frmM.legend1.Map.Redraw();
                    }
                }
                return sf;
            }
            catch
            {
                return null;
                //MessageBox.Show(err.Message);
            }
        }

        /// <summary>
        /// loading building shapefiles dynamically
        /// </summary>
        /// <param name="strbrgynm"></param>
        /// <param name="strbrgyno"></param>
        public Shapefile loadbldgpt(string strbrgynm, string strbrgyno, string dsnOLEDB)
        {
            try
            {
                string strno = String.Empty;
                if (strbrgyno.Length == 3)
                    strno = strbrgyno.Substring(1, 2);

                //new shapefile to be created
                Shapefile sf = new MapWinGIS.Shapefile();
                if (System.IO.File.Exists("b" + strno + "_pt.shp"))
                    System.IO.File.Delete("b" + strno + "_pt.shp");
                if (System.IO.File.Exists("b" + strno + "_pt.dbf"))
                    System.IO.File.Delete("b" + strno + "_pt.dbf");
                if (System.IO.File.Exists("b" + strno + "_pt.shx"))
                    System.IO.File.Delete("b" + strno + "_pt.shx");
                if (System.IO.File.Exists("b" + strno + "_pt.prj"))
                    System.IO.File.Delete("b" + strno + "_pt.prj");

                bool result = sf.CreateNew("b" + strno + "_pt.shp", MapWinGIS.ShpfileType.SHP_POINT);

                int intfld = 0;
                if (result == true)
                    result = sf.StartEditingShapes(true, null);
                //create fieldnames
                MapWinGIS.Field fld = new MapWinGIS.FieldClass();
                fld.Name = "BLDG_PIN";
                fld.Width = 23;
                fld.Type = MapWinGIS.FieldType.STRING_FIELD;
                sf.EditInsertField(fld, ref intfld, null);

                intfld += 1;
                fld = new MapWinGIS.FieldClass();
                fld.Name = "LAND_PIN";
                fld.Width = 18;
                fld.Type = MapWinGIS.FieldType.STRING_FIELD;
                sf.EditInsertField(fld, ref intfld, null);

                intfld += 1;
                fld = new MapWinGIS.FieldClass();
                fld.Name = "BLDG_CNT";
                fld.Width = 6;
                fld.Type = MapWinGIS.FieldType.INTEGER_FIELD;
                sf.EditInsertField(fld, ref intfld, null);

                intfld += 1;
                fld = new MapWinGIS.FieldClass();
                fld.Name = "X";
                fld.Width = 10;
                fld.Precision = 6;
                fld.Type = MapWinGIS.FieldType.DOUBLE_FIELD;
                sf.EditInsertField(fld, ref intfld, null);

                intfld += 1;
                fld = new MapWinGIS.FieldClass();
                fld.Name = "Y";
                fld.Width = 10;
                fld.Precision = 6;
                fld.Type = MapWinGIS.FieldType.DOUBLE_FIELD;
                sf.EditInsertField(fld, ref intfld, null);

                //here we need to create a shapefile that would be based on the number of
                //declared improvements from the rpta database
                //but first we would need the list of land pins with improvements within the barangay
                //ArrayList bldgList = populateBrgyBldg(dsnOLEDB, strbrgyno, 0);//ver 1
                ArrayList bldgList = populateBrgyBldgII(dsnOLEDB, strbrgyno, 0);//ver 2
                //ArrayList bldgcntlist = populateBrgyBldg(dsnOLEDB, strbrgyno, 1);//ver 1
                if (bldgList.Count == 0)
                {
                    MessageBox.Show("No declared building were found for the selected barangay.");
                    return null;
                }
                else
                {
                    Shapefile landsf = null;
                    string brgylayer = String.Empty;
                    for (int i = 0; i < frmM.legend1.Layers.Count; i++)
                    {
                        if (frmM.legend1.Map.get_LayerName(i).ToUpper() == "BARANGAY " + strbrgynm.ToUpper())
                        {
                            landsf = (Shapefile)frmM.axMap.get_GetObject(i);
                            break;
                        }
                    }

                    if (landsf != null)
                    {
                        //before loading the building point: you need
                        //the land parcel boundary of the selected barangay
                        //where we will get its tied land parcel(centroids) 
                        string strLayerName = "BLDG: " + strbrgynm.ToUpper();
                        string strfldname = "PIN";
                        int iFldInx = -1;
                        Color cfontcolor = Color.White;
                        //int intfontsize = 4;
                        iFldInx = landsf.Table.get_FieldIndexByName(strfldname);

                        if (iFldInx == -1)
                        {
                            MessageBox.Show("Please check the " + strLayerName + " layer, the barangay name field is missing.", "Confirm");
                        }
                        else
                        {
                            bool boolSuccess = false;
                            string pinval = String.Empty;
                            int ptref = 0;
                            //version 1:
                            //add only 1 pt and uses the land shape centroid
                            //for (int iii = 0; iii < landsf.NumShapes; iii++)
                            //{
                            //    pinval = landsf.get_CellValue(iFldInx, iii).ToString();

                            //    for (int ii = 0; ii < bldgList.Count; ii++)
                            //    {
                            //        if (bldgList[ii].ToString() == pinval)
                            //        {
                            //            Shape sh = landsf.get_Shape(iii);
                            //            MapWinGIS.Point shpt = sh.Centroid;

                            //            Shape sh1 = new MapWinGIS.ShapeClass();
                            //            boolSuccess = sh1.Create(MapWinGIS.ShpfileType.SHP_POINT);
                            //            ptref = ptref + 1;
                            //            //if (boolSuccess)
                            //                boolSuccess = sh1.InsertPoint(shpt, ref ptref);
                            //            //if (boolSuccess)
                            //                boolSuccess = sf.EditInsertShape(sh1, ref ptref);
                            //            //if (boolSuccess)
                            //            //{
                            //                bool boolCellAdd;
                            //                boolCellAdd = sf.EditCellValue(0, ptref, bldgList[ii]);
                            //                boolCellAdd = sf.EditCellValue(1, ptref, bldgcntlist[ii]);
                            //            //}

                            //            break;
                            //        }
                            //    }
                            //}

                            //version 2:
                            //add random pt and uses the land shape as its boundaries
                            string lastpin = String.Empty;

                            for (int ii = 0; ii < bldgList.Count; ii++)
                            {
                                int cntr = 1;
                                for (int iii = 0; iii < landsf.NumShapes; iii++)
                                {
                                    pinval = landsf.get_CellValue(iFldInx, iii).ToString();
                                    if (bldgList[ii].ToString().Substring(0, 18) == pinval)
                                    {
                                        MapWinGIS.Shape sh = landsf.get_Shape(iii);
                                        MapWinGIS.Point shpt = sh.Center;
                                        if (pinval == lastpin)
                                        {
                                            cntr += 1;
                                            Random rnd = new Random(DateTime.Now.Millisecond);
                                            shpt.x = sh.Extents.xMin + (sh.Extents.xMax - sh.Extents.xMin) * (rnd.NextDouble());
                                            shpt.y = sh.Extents.yMin + (sh.Extents.yMax - sh.Extents.yMin) * (rnd.NextDouble());
                                            //new Coordinate(XMIN + rnd.NextDouble() * XSPAN, YMIN + rnd.NextDouble() * YSPAN);

                                            //double radius = rnd.Next() * 90;
                                            //shpt.x = shpt.x + radius * Math.Cos(Math.PI / 18);
                                            //shpt.y = shpt.y + radius * Math.Sin(Math.PI / 18);
                                        }

                                        lastpin = pinval;
                                        MapWinGIS.Shape sh1 = new MapWinGIS.ShapeClass();
                                        boolSuccess = sh1.Create(MapWinGIS.ShpfileType.SHP_POINT);
                                        ptref = ptref + 1;
                                        //if (boolSuccess)
                                        //if(MapWinGeoProc.Utils.PointInPolygon(ref sh,ref shpt)==true)
                                        boolSuccess = sh1.InsertPoint(shpt, ref ptref);
                                        //if (boolSuccess)
                                        boolSuccess = sf.EditInsertShape(sh1, ref ptref);
                                        //if (boolSuccess)
                                        //{
                                        bool boolCellAdd;
                                        boolCellAdd = sf.EditCellValue(0, ptref, bldgList[ii]);
                                        boolCellAdd = sf.EditCellValue(1, ptref, bldgList[ii].ToString().Substring(0, 18));
                                        boolCellAdd = sf.EditCellValue(3, ptref, shpt.x);
                                        boolCellAdd = sf.EditCellValue(4, ptref, shpt.y);
                                        //boolCellAdd = sf.EditCellValue(1, ptref, bldgcntlist[ii]);//remarked for ver 2
                                        //}

                                        break;
                                    }
                                }
                            }

                            boolSuccess = sf.StopEditingShapes(true, true, null);
                            sf.SaveAs(frmM.m_shapebldglocation + @"\" + sf.Filename, null);

                            sf.Open(sf.Filename, null);

                            //setting the categories for unique values and gradient legend
                            sf = legendPointSingle(sf, Convert.ToUInt32(ColorTranslator.ToOle(Color.LightGray)), Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray)), true, (double)1, (double)1);
                            //setting the labels
                            //sf = setlabel2(sf, iFldInx, "ARIAL", cfontcolor, intfontsize, true, false, tkLabelFrameType.lfRectangle, Convert.ToUInt32(ColorTranslator.ToOle(Color.DarkGreen)), Convert.ToUInt32(ColorTranslator.ToOle(Color.PaleGreen)), false, false);

                            //adding the shapefile as layer
                            frmM.hndMap = frmM.legend1.Layers.Add(sf, true);
                            frmM.legend1.Layers.MoveLayer(frmM.hndMap, frmM.m_groupvector, frmM.legend1.Groups[frmM.m_groupvector].LayerCount);
                            frmM.legend1.Map.set_LayerName(frmM.hndMap, strLayerName);
                            frmM.legend1.Layers.ItemByHandle(frmM.hndMap).Expanded = false;
                            frmM.legend1.Layers.ItemByHandle(frmM.hndMap).Refresh();
                        }

                        frmM.hndMapSel = frmM.hndMap;
                        frmM.m_sellayername = strLayerName;
                        frmM.legend1.Map.Redraw();
                    }
                }
                return sf;
            }
            catch
            {
                return null;
                //MessageBox.Show(err.Message);
            }
        }

        public Shapefile loadbldg(string strbrgynm, string strbrgyno)
        {
            string strLayerName = String.Empty;
            Color cfontcolor = Color.White;
            int iFldInx = -1;
            int intfontsize = 8;
            string strno = String.Empty;

            if (strbrgyno.Length == 3)
                strno = strbrgyno.Substring(1, 2);
            Shapefile sf = new MapWinGIS.Shapefile();
            try
            {
                if (sf!=null)
                {
                    bool boolok = sf.Open(frmM.m_shapebldglocation + @"\" + "b" + strno + ".shp", null);
                    strLayerName = "BLDG: " + strbrgynm.ToUpper();
                    string strfldname = String.Empty;//"PIN";
                    string sqry = String.Format("[LAYER_NAME] = '{0}'", strLayerName);
                    DataRow[] foundRows = frmM.m_dtlayer.Select(sqry);
                    if (foundRows.Length > 0)
                    {
                        foreach (DataRow dr in foundRows)
                        {
                            strfldname = dr["CATEGORY"].ToString().Trim();
                        }
                    }
                    iFldInx = sf.Table.get_FieldIndexByName(strfldname);
                }
                string description = String.Empty;
                frmM.hndMap = frmM.legend1.Layers.Add(sf, true);
                bool boolcatexit = frmM.legend1.Map.LoadLayerOptions(frmM.hndMap, "", ref description); ////will get the xml default options
                if (boolcatexit == false)
                {
                    //setting the categories for unique values and gradient legend
                    sf = legendPolygonSingle(sf, Convert.ToUInt32(ColorTranslator.ToOle(Color.Red)), (float)1, false, (UInt32)(ColorTranslator.ToOle(Color.Transparent)));
                    if(sf!=null)
                        sf = setlabel2(sf, iFldInx, "ARIAL", cfontcolor, intfontsize, true, false, tkLabelFrameType.lfRectangle, Convert.ToUInt32(ColorTranslator.ToOle(Color.DarkGreen)), Convert.ToUInt32(ColorTranslator.ToOle(Color.PaleGreen)), false, false);
                }
                frmM.legend1.Layers.MoveLayer(frmM.hndMap, frmM.m_groupvector, frmM.legend1.Groups[frmM.m_groupvector].LayerCount);
                frmM.legend1.Map.set_LayerName(frmM.hndMap, strLayerName);
                frmM.legend1.Layers.ItemByHandle(frmM.hndMap).Expanded = false;
                frmM.legend1.Layers.ItemByHandle(frmM.hndMap).Refresh();

                frmM.hndMapSel = frmM.hndMap;
                frmM.m_sellayername = strLayerName;
                frmM.legend1.Map.Redraw();

                return sf;
            }
            catch
            {
                return sf;
            }
        }

        public ArrayList populateBrgyBldg(string dsnOLEDB, string strbrgyno,int cntrl)
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            ArrayList bldglist = new ArrayList();
            ArrayList bldgcntlist = new ArrayList();
            try
            {
                StringBuilder sqry = new StringBuilder();
                sqry.Append("select substr(trim(bldg_pin),0,18) ");
                sqry.Append(string.Format("landpin, count(trim(bldg_pin)) as bldgcnt from {0}.faas_bldg where substr(trim(bldg_pin),8,3) = '{1}' ", frmM.m_rptdbase, strbrgyno));
                sqry.Append("group by substr(trim(bldg_pin),0,18) ");
                sqry.Append("order by trim(bldg_pin) asc");
                OleDbConnection oledbconn = new OleDbConnection(frmM.m_strconn);
                OleDbCommand cmd = new OleDbCommand(sqry.ToString(), oledbconn);
                OleDbDataAdapter newDataAdapter = new OleDbDataAdapter(cmd);
                DataSet newDataSet = new DataSet();
                oledbconn.Open();
                OleDbDataReader newDataReaders = cmd.ExecuteReader();

                if (newDataReaders.HasRows)
                {
                    //String strVal = "";
                    while (newDataReaders.Read())
                    {
                        if (!DBNull.Value.Equals(newDataReaders["LANDPIN"]))
                        {
                            bldglist.Add((String)newDataReaders["LANDPIN"].ToString());
                            bldgcntlist.Add((String)newDataReaders["BLDGCNT"].ToString());
                        }
                    }
                    newDataReaders.Close();
                    oledbconn.Close();
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                    //return bldglist;
                    if (cntrl == 0)
                        return bldglist;
                    else
                        return bldgcntlist;
                }
                else
                {
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                    //return bldglist;
                    if (cntrl == 0)
                        return bldglist;
                    else
                        return bldgcntlist;
                }

            }
            catch
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                //return bldglist;
                if (cntrl == 0)
                    return bldglist;
                else
                    return bldgcntlist;
            }

        }
        public ArrayList populateBrgyBldgII(string dsnOLEDB, string strbrgyno, int cntrl)
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            ArrayList bldglist = new ArrayList();
            ArrayList bldgcntlist = new ArrayList();
            try
            {
                StringBuilder sqry = new StringBuilder();
                sqry.Append("select trim(bldg_pin) bldg_pin ");
                sqry.Append(string.Format("from {0}.faas_bldg where substr(trim(bldg_pin),8,3) = '{1}' ", frmM.m_rptdbase, strbrgyno));
                //sqry.Append("group by substr(bldg_pin,0,18) ");
                sqry.Append("order by trim(bldg_pin) asc");
                OleDbConnection oledbconn = new OleDbConnection(frmM.m_strconn);
                OleDbCommand cmd = new OleDbCommand(sqry.ToString(), oledbconn);
                OleDbDataAdapter newDataAdapter = new OleDbDataAdapter(cmd);
                DataSet newDataSet = new DataSet();
                oledbconn.Open();
                OleDbDataReader newDataReaders = cmd.ExecuteReader();

                if (newDataReaders.HasRows)
                {
                    //String strVal = "";
                    while (newDataReaders.Read())
                    {
                        if (!DBNull.Value.Equals(newDataReaders["BLDG_PIN"]))
                        {
                            bldglist.Add((String)newDataReaders["BLDG_PIN"].ToString());
                            //bldgcntlist.Add((String)newDataReaders["BLDGCNT"].ToString());
                        }
                    }
                    newDataReaders.Close();
                    oledbconn.Close();
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                    //return bldglist;
                    if (cntrl == 0)
                        return bldglist;
                    else
                        return bldgcntlist;
                }
                else
                {
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                    //return bldglist;
                    if (cntrl == 0)
                        return bldglist;
                    else
                        return bldgcntlist;
                }

            }
            catch
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                //return bldglist;
                if (cntrl == 0)
                    return bldglist;
                else
                    return bldgcntlist;
            }

        }
        public void DrawShape(MapWinGIS.Point prevpt, MapWinGIS.Point newpt)
        {
            //draw_hndl = frmM.axMap.NewDrawing(MapWinGIS.tkDrawReferenceList.dlSpatiallyReferencedList);
            if (prevpt != null)
                frmM.axMap.DrawLine(prevpt.x, prevpt.y, newpt.x, newpt.y, (int)3, Convert.ToUInt32(ColorTranslator.ToOle(Color.Black)));
        }

        /// <summary>
        /// ver 1: 
        /// working for 1 polygon and 1 polylineline
        /// </summary>
        /// <param name="s"></param>
        /// <param name="dsnOLEDB"></param>
        public void VerifyShape(MapWinGIS.Shape s, string dsnOLEDB)
        {
            try
            {
                //this arraylist will be the reference for the removal/undo of the subd of parcels
                ArrayList newshpindxcoll = new ArrayList();
                string strmotherpin = String.Empty;
                int intmotheridx = -1;
                if (s.numPoints < 2)
                {
                    resetLine("SUBDLAND");
                    Logger.Message("You must add at least two points for a line.", "Not Enough Points", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, DialogResult.OK);
                    return;
                }

                Shapefile newsf = new MapWinGIS.Shapefile();
                object ShapeIDS = new object();
                Shapefile sf = (Shapefile)frmM.axMap.get_GetObject(frmM.hndMapSel);
                Extents ext = frmM.m_newlineshape.Extents;

                if (System.IO.File.Exists("new_temp_subd_shp.shp"))
                    System.IO.File.Delete("new_temp_subd_shp.shp");
                if (System.IO.File.Exists("new_temp_subd_shp.dbf"))
                    System.IO.File.Delete("new_temp_subd_shp.dbf");
                if (System.IO.File.Exists("new_temp_subd_shp.shx"))
                    System.IO.File.Delete("new_temp_subd_shp.shx");
                if (System.IO.File.Exists("new_temp_subd_shp.prj"))
                    System.IO.File.Delete("new_temp_subd_shp.prj");

                if (sf.SelectShapes(ext, (double)0, SelectMode.INTERSECTION, ref ShapeIDS))
                {
                    int[] shapes = (int[])ShapeIDS;
                    if (shapes.Length > 0)
                    {
                        for (int i = 0; i < shapes.Length; i++)
                        {
                            bool results = newsf.CreateNew("new_temp_subd_shp.shp", MapWinGIS.ShpfileType.SHP_POLYGON);
                            MapWinGIS.Shape polyshp = sf.get_Shape(Convert.ToInt32(shapes.GetValue(i)));
                            MapWinGIS.Shape lineshp = frmM.m_newlineshape;
                            strmotherpin = sf.Table.get_CellValue(sf.Table.get_FieldIndexByName("PIN"),Convert.ToInt32(shapes.GetValue(i))).ToString();
                            intmotheridx = Convert.ToInt32(shapes.GetValue(i));
                            if (MapWinGeoProc.SpatialOperations.ClipPolygonWithLine(ref polyshp, ref lineshp, out newsf))
                            {
                                int nextpinsw = 0;
                                string lastpin = String.Empty;
                                for (int ii = 0; ii <= newsf.NumShapes-1; ii++)
                                {
                                    int newshpidx = sf.NumShapes;
                                    MapWinGIS.Shape sh = newsf.get_Shape(ii);
                                    sf.StartEditingShapes(true, null);
                                    if (sf.EditInsertShape(sh, ref newshpidx) == false)
                                    {
                                        MessageBox.Show("Failed to add the new shape to the shapefile.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    else
                                    {
                                        if (sf.EditingShapes == true)
                                        {
                                            newshpindxcoll.Add(newshpidx);
                                            for (int fldidx = 0; fldidx < sf.NumFields; fldidx++)
                                            {
                                                if (sf.get_Field(fldidx).Name.ToUpper() != "SHAPE_ID" || sf.get_Field(fldidx).Name.ToUpper() != "")
                                                {
                                                    object sval = sf.get_CellValue(fldidx, Convert.ToInt32(shapes.GetValue(i)));
                                                    if (sf.Table.get_Field(fldidx).Type == FieldType.STRING_FIELD)
                                                    {
                                                        if (sval == null)
                                                        {
                                                            sval = (String)"";
                                                        }
                                                        else
                                                        {
                                                            string ssect = String.Empty;
                                                            if (sf.get_Field(fldidx).Name == "PIN")
                                                            {
                                                                if (sval.ToString() == "")
                                                                {
                                                                    if (nextpinsw == 0)
                                                                    {
                                                                        //get dbase faas_land table last parcel pin of the sect
                                                                        //based on the selected section boundary e.g. 002-05-001-001-099
                                                                        sval = GetPinFromSectBndry(ext, "SECTION BOUNDARY");
                                                                        sval = GetDBaseLastPin(sval.ToString(), dsnOLEDB);
                                                                        nextpinsw = 1;
                                                                        lastpin = sval.ToString();
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (nextpinsw == 0)
                                                                    {
                                                                        //get dbase faas_land table last parcel pin of the sect
                                                                        //based on the selected parcel boundary e.g. 001
                                                                        sval = GetDBaseLastPin(sval.ToString(), dsnOLEDB);
                                                                        if (sval.ToString().Length == 18 && sval.ToString() != "")
                                                                        {
                                                                            //double result;
                                                                            //if (double.TryParse(ssect, out result) == true)
                                                                            //{
                                                                                //sval = sval.ToString().Substring(0, 15) + String.Format("{0:D3}", Convert.ToInt32(ssect) + 1);
                                                                                nextpinsw = 1;
                                                                                lastpin = sval.ToString();
                                                                            //}
                                                                        }
                                                                    }
                                                                }
                                                                sval = lastpin.Substring(0, 15) + String.Format("{0:D3}", Convert.ToInt32(lastpin.Substring(15, 3)) + 1);
                                                                //get gis shapefile table last parcel pin of the section
                                                                //based on the selected land parcel boundary
                                                                // try to validate if the new suggested pin from the database 
                                                                // are already existing in the gis land parcel shapefile
                                                                string error = "";
                                                                object result = null;
                                                                if (sf.Table.Query("[PIN] = \"" + sval + "\"", ref result, ref error))
                                                                {
                                                                    int[] shapes2 = result as int[];
                                                                    if (shapes2 != null)
                                                                    {
                                                                        //the suggested pin from the dbase is already existing in the shapefile
                                                                        //get new suggested pin from the gis shapefile
                                                                        shapes2 = null;
                                                                        StringBuilder sqry = new StringBuilder();
                                                                        sqry.Append(string.Format("[BRGY] = \"{0}\"",sval.ToString().Substring(7,3)));
                                                                        sqry.Append(string.Format(" AND [SECT] = \"{0}\"",sval.ToString().Substring(11,3)));
                                                                        sqry.Append(string.Format(" AND [PRCL] <> \"{0}\"", "999"));
                                                                        if (sf.Table.Query(sqry.ToString(), ref result, ref error))
                                                                        {
                                                                            shapes2 = (int[])result;
                                                                            ArrayList prclColl = new ArrayList();
                                                                            int intprcl;
                                                                            for (int iii = 0; iii < shapes2.Length; iii++)
                                                                            {
                                                                                int fidx = -1;
                                                                                fidx = sf.Table.get_FieldIndexByName("PRCL");
                                                                                if (fidx != -1)
                                                                                {
                                                                                    prclColl.Add(sf.get_CellValue(fidx, Convert.ToInt32(shapes2.GetValue(iii).ToString())).ToString());
                                                                                }
                                                                            }
                                                                            prclColl.Sort();
                                                                            prclColl.Reverse();
                                                                            if (int.TryParse(prclColl[0].ToString(), out intprcl))
                                                                                sval = lastpin.Substring(0, 15) + String.Format("{0:D3}", Convert.ToInt32(prclColl[0].ToString()) + 1);
                                                                        }
                                                                        //else
                                                                        //{
                                                                        //    //if the validation is false or no records were found in the shapefile
                                                                        //}
                                                                    }
                                                                }

                                                                lastpin = sval.ToString();
                                                            }
                                                            else if (sf.get_Field(fldidx).Name == "PRCL")
                                                                sval = lastpin.ToString().Substring(15, 3);
                                                        }
                                                    }
                                                    else if (sf.Table.get_Field(fldidx).Type == FieldType.DOUBLE_FIELD)
                                                    {
                                                        if (sval == null)
                                                        {
                                                            sval = (double)0;
                                                        }
                                                        else
                                                        {
                                                            if (sf.get_Field(fldidx).Name.ToUpper() == "AREA")
                                                                sval = sh.Area;
                                                            else if (sf.get_Field(fldidx).Name.ToUpper() == "PERIMETER")
                                                                sval = sh.Perimeter;
                                                        }
                                                    }
                                                    else if (sf.Table.get_Field(fldidx).Type == FieldType.INTEGER_FIELD)
                                                    {
                                                        if (sval == null)
                                                        {
                                                            sval = (int)0;
                                                        }
                                                        else
                                                        {
                                                            if (sf.get_Field(fldidx).Name.ToUpper() == "CORNER_LOT")
                                                                sval = (int)0;
                                                        }
                                                    }
                                                    sf.EditCellValue(fldidx, newshpidx, sval);
                                                }
                                            }
                                            //create a backup of shapefile here before saving anything
                                            sf.StopEditingShapes(true, true, null);
                                        }
                                    }
                                }
                                //resetLine();
                                //call the subdivision form
                                if (frmsubd == null)
                                {
                                    frmsubd = new frmSubdivideLand2();
                                    frmsubd.Owner = frmM;
                                }

                                if(intmotheridx != -1)
                                    frmsubd.frmInit(frmM, this, sf, newshpindxcoll, strmotherpin, intmotheridx);
                                
                            }
                            else
                            {
                                resetLine("SUBDLAND");
                                MessageBox.Show("Failed to add the new shape to the shapefile. \n\rPlease check your division line.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                            //newsf.NumShapes
                            //newsf.SaveAs(newsf.Filename, null);
                        }

                    }
                }
            }
            catch (Exception err)
            {
                resetLine("SUBDLAND");
                MessageBox.Show(err.Message);
            }

        }

        /// <summary>
        /// test function to cut multi polygon
        /// </summary>
        /// <param name="s"></param>
        /// <param name="dsnOLEDB"></param>
        public void VerifyShape2(MapWinGIS.Shape s, string dsnOLEDB)
        {
            
            //this arraylist will be the reference for the removal/undo of the subd of parcels

            ArrayList newshpindxcoll = new ArrayList();//edited 10/17/2013
            ArrayList mothershpindxcoll = new ArrayList();//edited 10/17/2013
            if (frmsubd != null)
            {
                if (frmsubd.Visible == false)
                {
                    newshpindxcoll = new ArrayList();
                    mothershpindxcoll = new ArrayList();
                }
            }
            //ArrayList newshpindxcoll = new ArrayList();////remarked 9/17/2013
            //ArrayList mothershpindxcoll = new ArrayList();////remarked 9/17/2013
            string strmotherpin = String.Empty;
            int intmotheridx = -1;
            if (s.numPoints < 2)
            {
                resetLine("SUBDLAND");
                Logger.Message("You must add at least two points for a line.", "Not Enough Points", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, DialogResult.OK);
                return;
            }

            Shapefile newsf = new MapWinGIS.Shapefile();
            if (System.IO.File.Exists("new_temp_subd_shp.shp"))
                System.IO.File.Delete("new_temp_subd_shp.shp");
            if (System.IO.File.Exists("new_temp_subd_shp.dbf"))
                System.IO.File.Delete("new_temp_subd_shp.dbf");
            if (System.IO.File.Exists("new_temp_subd_shp.shx"))
                System.IO.File.Delete("new_temp_subd_shp.shx");
            if (System.IO.File.Exists("new_temp_subd_shp.prj"))
                System.IO.File.Delete("new_temp_subd_shp.prj");

            bool results = newsf.CreateNew("new_temp_subd_shp.shp", MapWinGIS.ShpfileType.SHP_POLYGON);
            object ShapeIDS = new object();
            Shapefile sf = (Shapefile)frmM.axMap.get_GetObject(frmM.hndMapSel);
            Extents ext = frmM.m_newlineshape.Extents;

            try
            {
                
                if (sf.SelectShapes(ext, (double)0, SelectMode.INTERSECTION, ref ShapeIDS))
                {
                    int[] shapes = (int[])ShapeIDS;
                    if (shapes.Length > 0)
                    {
                        string lastpin = String.Empty;
                        for (int i = 0; i < shapes.Length; i++)
                        {
                            MapWinGIS.Shape polyshp = sf.get_Shape(Convert.ToInt32(shapes.GetValue(i)));
                            MapWinGIS.Shape lineshp = frmM.m_newlineshape;
                            strmotherpin = sf.Table.get_CellValue(sf.Table.get_FieldIndexByName("PIN"), Convert.ToInt32(shapes.GetValue(i))).ToString();
                            intmotheridx = Convert.ToInt32(shapes.GetValue(i));
                            //if (MapWinGeoProc.SpatialOperations.ClipPolygonWithLine(ref polyshp, ref lineshp, out newsf))
                            //if(MapWinGeoProc.SpatialOperations.c
                            //MapWinGIS.Utils util = new Utils();
                            //util.ClipPolygon(PolygonOperation.

                            //MapWinGeoProc.SpatialOperations
                            //MapWinGeoProc.SpatialOperations.ClipPolygonWithLine

                            try
                            {
                                
                                if (MapWinGeoProc.SpatialOperations.ClipPolygonWithLine(ref polyshp, ref lineshp, out newsf))
                                {


                                    int nextpinsw = 0;

                                    for (int ii = 0; ii < newsf.NumShapes; ii++)
                                    {
                                        int newshpidx = sf.NumShapes;
                                        MapWinGIS.Shape sh = newsf.get_Shape(ii);
                                        sf.StartEditingShapes(true, null);
                                        if (sf.EditInsertShape(sh, ref newshpidx) == false)
                                        {
                                            MessageBox.Show("Failed to add the new shape to the shapefile.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            sf.StopEditingShapes(false, true, null);
                                            return;
                                        }
                                        else
                                        {
                                            if (sf.EditingShapes == true)
                                            {
                                                mothershpindxcoll.Add(intmotheridx);
                                                newshpindxcoll.Add(newshpidx);
                                                for (int fldidx = 0; fldidx < sf.NumFields; fldidx++)
                                                {
                                                    if (sf.get_Field(fldidx).Name.ToUpper() != "SHAPE_ID" || sf.get_Field(fldidx).Name.ToUpper() != "")
                                                    {
                                                        object sval = sf.get_CellValue(fldidx, Convert.ToInt32(shapes.GetValue(i)));
                                                        if (sf.Table.get_Field(fldidx).Type == FieldType.STRING_FIELD)
                                                        {
                                                            if (sval == null)
                                                            {
                                                                sval = (String)"";
                                                            }
                                                            else
                                                            {
                                                                //string ssect = String.Empty;
                                                                if (sf.get_Field(fldidx).Name == "PIN")
                                                                {
                                                                    if (lastpin != String.Empty)
                                                                    {
                                                                        if (sval.ToString().Substring(0, 14) != lastpin.Substring(0, 14))
                                                                            nextpinsw = 0;
                                                                    }
                                                                    if (nextpinsw == 0)
                                                                    {
                                                                        if (sval.ToString() == "")
                                                                        {

                                                                            //get dbase faas_land table last parcel pin of the sect
                                                                            //based on the selected section boundary e.g. 002-05-001-001-099
                                                                            //not yet tested
                                                                            lastpin = GetPinFromSectBndry(ext, "SECTION BOUNDARY");
                                                                            lastpin = GetDBaseLastPin(lastpin, dsnOLEDB);
                                                                            nextpinsw = 1;
                                                                            //lastpin = sval.ToString();
                                                                        }
                                                                        else
                                                                        {
                                                                            //get dbase faas_land table last parcel pin of the sect
                                                                            //based on the selected parcel boundary e.g. 001
                                                                            lastpin = GetDBaseLastPin(sval.ToString(), dsnOLEDB);
                                                                            //if (sval.ToString().Length == 18 && sval.ToString() != "")
                                                                            //{
                                                                            //    //double result;
                                                                            //    //if (double.TryParse(ssect, out result) == true)
                                                                            //    //{
                                                                            //    //sval = sval.ToString().Substring(0, 15) + String.Format("{0:D3}", Convert.ToInt32(ssect) + 1);
                                                                            nextpinsw = 1;
                                                                            //    //lastpin = sval.ToString();
                                                                            //    //}
                                                                            //}
                                                                        }
                                                                        //the last dbase pin
                                                                        sval = lastpin;
                                                                        //sval = lastpin.Substring(0, 15) + String.Format("{0:D3}", Convert.ToInt32(lastpin.Substring(15, 3)));
                                                                        //get gis shapefile table last parcel pin of the section
                                                                        //based on the selected land parcel boundary
                                                                        // try to validate if the new suggested pin from the database 
                                                                        // are already existing in the gis land parcel shapefile
                                                                        string error = "";
                                                                        object result = null;
                                                                        if (sf.Table.Query("[PIN] = \"" + sval.ToString() + "\"", ref result, ref error))
                                                                        {
                                                                            int[] shapes2 = result as int[];
                                                                            if (shapes2 != null)
                                                                            {
                                                                                //the suggested pin from the dbase is already existing in the shapefile
                                                                                //get new suggested pin from the gis shapefile
                                                                                shapes2 = null;
                                                                                string error2 = "";
                                                                                object result2 = null;
                                                                                StringBuilder sqry = new StringBuilder();
                                                                                sqry.Append(string.Format("[BRGY] = \"{0}\"", sval.ToString().Substring(7, 3)));
                                                                                sqry.Append(string.Format(" AND [SECT] = \"{0}\"", sval.ToString().Substring(11, 3)));
                                                                                sqry.Append(string.Format(" AND [PRCL] <> \"{0}\"", "999"));
                                                                                if (sf.Table.Query(sqry.ToString(), ref result2, ref error2))
                                                                                {
                                                                                    shapes2 = (int[])result2;
                                                                                    ArrayList prclColl = new ArrayList();
                                                                                    int intprcl;
                                                                                    for (int iii = 0; iii < shapes2.Length; iii++)
                                                                                    {
                                                                                        int fidx = -1;
                                                                                        fidx = sf.Table.get_FieldIndexByName("PRCL");
                                                                                        if (fidx != -1)
                                                                                        {
                                                                                            prclColl.Add(sf.get_CellValue(fidx, Convert.ToInt32(shapes2.GetValue(iii).ToString())).ToString());
                                                                                        }
                                                                                    }
                                                                                    prclColl.Sort();
                                                                                    prclColl.Reverse();
                                                                                    if (int.TryParse(prclColl[0].ToString(), out intprcl))
                                                                                        lastpin = sval.ToString().Substring(0, 15) + String.Format("{0:D3}", Convert.ToInt32(prclColl[0].ToString()));
                                                                                    //lastpin = sval.ToString();
                                                                                }
                                                                                else
                                                                                {
                                                                                    lastpin = sval.ToString();
                                                                                    //    //if the validation is false or no records were found in the shapefile
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            //test if the dbase suggested pin is the greater than the last pin in the shapefile
                                                                            //dbase pin vs shapefile pin
                                                                            int dbasepin = -1;
                                                                            int shapepin = -1;
                                                                            if (IsNumeric(sval.ToString().Substring(15, 3)))
                                                                                dbasepin = Convert.ToInt32(sval.ToString().Substring(15, 3));

                                                                            //get shapefile last prcl of sect
                                                                            StringBuilder sqry2 = new StringBuilder();
                                                                            sqry2.Append(string.Format("[BRGY] = \"{0}\"", sval.ToString().Substring(7, 3)));
                                                                            sqry2.Append(string.Format(" AND [SECT] = \"{0}\"", sval.ToString().Substring(11, 3)));
                                                                            sqry2.Append(string.Format(" AND [PRCL] <> \"{0}\"", "999"));
                                                                            if (sf.Table.Query(sqry2.ToString(), ref result, ref error))
                                                                            {
                                                                                int[] shapes3 = result as int[];
                                                                                ArrayList prclColl2 = new ArrayList();
                                                                                int intprcl;
                                                                                for (int iii = 0; iii < shapes3.Length; iii++)
                                                                                {
                                                                                    int fidx = -1;
                                                                                    fidx = sf.Table.get_FieldIndexByName("PRCL");
                                                                                    if (fidx != -1)
                                                                                        prclColl2.Add(sf.get_CellValue(fidx, Convert.ToInt32(shapes3.GetValue(iii).ToString())).ToString());
                                                                                }
                                                                                prclColl2.Sort();
                                                                                prclColl2.Reverse();
                                                                                if (int.TryParse(prclColl2[0].ToString(), out intprcl))
                                                                                    shapepin = Convert.ToInt32(prclColl2[0].ToString());
                                                                                //sval = lastpin.Substring(0, 15) + String.Format("{0:D3}", Convert.ToInt32(prclColl[0].ToString()) + 1);

                                                                            }

                                                                            if (shapepin > dbasepin)
                                                                                lastpin = sval.ToString().Substring(0, 15) + String.Format("{0:D3}", shapepin);
                                                                            else
                                                                                lastpin = sval.ToString().Substring(0, 15) + String.Format("{0:D3}", dbasepin);

                                                                            //lastpin = sval.ToString();
                                                                        }
                                                                    }
                                                                    sval = lastpin.Substring(0, 15) + String.Format("{0:D3}", Convert.ToInt32(lastpin.Substring(15, 3)) + 1);
                                                                    lastpin = sval.ToString();
                                                                }
                                                                else if (sf.get_Field(fldidx).Name == "CITY")
                                                                    sval = lastpin.ToString().Substring(0, 3);
                                                                else if (sf.get_Field(fldidx).Name == "DIST")
                                                                    sval = lastpin.ToString().Substring(4, 2);
                                                                else if (sf.get_Field(fldidx).Name == "BRGY")
                                                                    sval = lastpin.ToString().Substring(7, 3);
                                                                else if (sf.get_Field(fldidx).Name == "SECT")
                                                                    sval = lastpin.ToString().Substring(11, 3);
                                                                else if (sf.get_Field(fldidx).Name == "PRCL")
                                                                    sval = lastpin.ToString().Substring(15, 3);
                                                            }
                                                        }
                                                        else if (sf.Table.get_Field(fldidx).Type == FieldType.DOUBLE_FIELD)
                                                        {
                                                            if (sval == null)
                                                            {
                                                                sval = (double)0;
                                                            }
                                                            else
                                                            {
                                                                //pending work to verify the area and perimeter of decimal degree projection
                                                                if (sf.get_Field(fldidx).Name.ToUpper() == "AREA")
                                                                {

                                                                    double dbArea = MapWinGeoProc.Utils.Area(ref sh, MapWindow.Interfaces.UnitOfMeasure.DecimalDegrees);
                                                                    dbArea = MapWinGeoProc.UnitConverter.ConvertArea(MapWindow.Interfaces.UnitOfMeasure.Kilometers, MapWindow.Interfaces.UnitOfMeasure.Meters, dbArea);
                                                                    sval = dbArea;//sval = sh.Area;
                                                                }
                                                                else if (sf.get_Field(fldidx).Name.ToUpper() == "PERIMETER")
                                                                {
                                                                    Utils util = new Utils();
                                                                    double dbPerimeter = util.get_Perimeter(sh);
                                                                    dbPerimeter = MapWinGeoProc.UnitConverter.ConvertLength(MapWindow.Interfaces.UnitOfMeasure.Kilometers, MapWindow.Interfaces.UnitOfMeasure.Meters, dbPerimeter);
                                                                    sval = dbPerimeter;//sval = sh.Perimeter;
                                                                }
                                                            }
                                                        }
                                                        else if (sf.Table.get_Field(fldidx).Type == FieldType.INTEGER_FIELD)
                                                        {
                                                            if (sval == null)
                                                            {
                                                                sval = (int)0;
                                                            }
                                                            else
                                                            {
                                                                if (sf.get_Field(fldidx).Name.ToUpper() == "CORNER_LOT")
                                                                    sval = (int)0;
                                                            }
                                                        }
                                                        sf.EditCellValue(fldidx, newshpidx, sval);
                                                    }
                                                }
                                                //create a backup of shapefile here before saving anything
                                                sf.StopEditingShapes(true, true, null);
                                            }
                                        }
                                    }
                                    //resetLine();
                                    ////remarked on ver 2
                                    ////call the subdivision form
                                    //if (frmsubd == null)
                                    //    frmsubd = new frmSubdivideLand2();

                                    //if (intmotheridx != -1)
                                    //    frmsubd.frmInit(frmM, this, sf, newshpindxcoll, strmotherpin, intmotheridx);

                                }
                                else
                                {
                                    ////remarked on ver 2
                                    ////resetLine();
                                    ////MessageBox.Show("Failed to add the new shape to the shapefile. \n\rPlease check your division line.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    //MessageBox.Show("liloy");
                                }
                            }
                            catch(Exception err)
                            {
                                MessageBox.Show(err.Message);
                                break;
                            }
                        }
                        //resetLine();
                        ////call the subdivision form
                        if (File.Exists(newsf.Filename))
                        {
                            string newshpfilename = newsf.Filename.ToString();
                            MapWinGeoProc.DataManagement.DeleteShapefile(ref newshpfilename);
                        }

                        if (frmsubd == null)
                            frmsubd = new frmSubdivideLand2();

                        if (mothershpindxcoll.Count > 0)
                            frmsubd.frmInit2(frmM, this, sf, newshpindxcoll, mothershpindxcoll);
                    }
                }
            }
            catch (Exception err)
            {
                //resetLine();
                foreach (ToolStripButton item in frmM.mutuallyExclusiveButtons)
                {
                    item.Checked = false;
                }
                if (sf.EditingShapes == true)
                    sf.StopEditingShapes(false, true, null);
                frmM.axMap.ClearDrawings();
                //frmM.m_mapcntrl = "";
                frmM.axMap.CursorMode = tkCursorMode.cmNone;
                frmM.axMap.SendMouseUp = false;
                frmM.axMap.MapCursor = tkCursor.crsrMapDefault;
                MessageBox.Show(err.Message);
            }

        }

        public void VerifyShape3(MapWinGIS.Shape s, string dsnOLEDB)
        {
            //this arraylist will be the reference for the removal/undo of the subd of parcels
            ArrayList newshpindxcoll = new ArrayList();
            ArrayList mothershpindxcoll = new ArrayList();
            ArrayList mothershpincoll = new ArrayList();
            ArrayList mothershpindxcoll2 = new ArrayList();
            string strmotherpin = String.Empty;
            string strtestsect = String.Empty;
            string strtestpin = String.Empty;
            string sdbaselastpin = String.Empty;
            string sshapelastpin = String.Empty;
            string ssuggestednextpin = String.Empty;
            Dictionary< string,int> pins = new Dictionary<string,int>();

            try
            {
                //int intmotheridx = -1;
                if (s.numPoints < 2)
                {
                    resetLine("CONSLAND");
                    Logger.Message("You must add at least two points for a line.", "Not Enough Points", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, DialogResult.OK);
                    return;
                }

                Shapefile newsf = new MapWinGIS.Shapefile();
                if (System.IO.File.Exists("new_temp_cons_shp.shp"))
                    System.IO.File.Delete("new_temp_cons_shp.shp");
                if (System.IO.File.Exists("new_temp_cons_shp.dbf"))
                    System.IO.File.Delete("new_temp_cons_shp.dbf");
                if (System.IO.File.Exists("new_temp_cons_shp.shx"))
                    System.IO.File.Delete("new_temp_cons_shp.shx");
                if (System.IO.File.Exists("new_temp_cons_shp.prj"))
                    System.IO.File.Delete("new_temp_cons_shp.prj");

                bool results = newsf.CreateNew("new_temp_cons_shp.shp", MapWinGIS.ShpfileType.SHP_POLYLINE);
                if (!results)
                {
                    MessageBox.Show("Failed to create a new shapefile. \n\r" + newsf.get_ErrorMsg(newsf.LastErrorCode), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    int intfld = 0;
                    //create fieldnames
                    MapWinGIS.Field fld = new MapWinGIS.FieldClass();
                    fld.Name = "MWShapeID";
                    fld.Width = 10;
                    fld.Type = MapWinGIS.FieldType.INTEGER_FIELD;
                    newsf.EditInsertField(fld, ref intfld, null);
                }

                object ShapeIDS = new object();
                Shapefile sf = (Shapefile)frmM.axMap.get_GetObject(frmM.hndMapSel);
                Extents ext = frmM.m_newlineshape.Extents;

                object newShapeIDS = new object();
                int newshpidx = 1;
                if (results)
                {
                    results = newsf.StartEditingShapes(true, null);
                    newsf.EditInsertShape(s, ref newshpidx);
                }
                if (results)
                    results = newsf.StopEditingShapes(true, true, null);
                sf.SelectByShapefile(newsf, tkSpatialRelation.srCrosses, false, ref newShapeIDS, null);
                int[] newselshapes = (int[])newShapeIDS;

                int pinfldidx = -1;
                pinfldidx = sf.Table.get_FieldIndexByName("PIN");
                if (pinfldidx == -1)
                {
                    MessageBox.Show("Can't find the PIN field, \n\rplease check the shapefile table.");
                    return;
                }

                for (int i = 0; i < newselshapes.Length; i++)
                {
                    strmotherpin = sf.Table.get_CellValue(pinfldidx, Convert.ToInt32(newselshapes.GetValue(i))).ToString();
                    if (strmotherpin != "")
                        mothershpincoll.Add(strmotherpin);
                    mothershpindxcoll.Add(Convert.ToInt32(newselshapes.GetValue(i)));
                    pins.Add(strmotherpin, Convert.ToInt32(newselshapes.GetValue(i)));
                }

                foreach (string strpin in mothershpincoll)
                {
                    //if(mothershpincoll[0].ToString().Length == 18)
                    if (strpin.Length == 18)
                    {
                        strtestpin = strpin;
                        strtestsect = strpin.Substring(0, 14);
                        break;
                    }
                }

                if (strtestsect != "")
                {
                    sdbaselastpin = GetDBaseLastPin(strtestpin, dsnOLEDB);
                    sshapelastpin = GetShapeLastPIN(strtestpin, sf);

                    if (Convert.ToInt32(sdbaselastpin.Substring(15, 3)) > Convert.ToInt32(sshapelastpin.Substring(15, 3)))
                        ssuggestednextpin = sdbaselastpin.Substring(0, 15) + String.Format("{0:D3}", Convert.ToInt32(sdbaselastpin.Substring(15, 3)) + 1);
                    else
                        ssuggestednextpin = sshapelastpin.Substring(0, 15) + String.Format("{0:D3}", Convert.ToInt32(sshapelastpin.Substring(15, 3)) + 1);
                }

                if (File.Exists(newsf.Filename))
                {
                    string newshpfilename = newsf.Filename.ToString();
                    MapWinGeoProc.DataManagement.DeleteShapefile(ref newshpfilename);
                }



                if (frmcons == null)
                {
                    frmcons = new frmConsLand();
                    frmcons.Owner = frmM;
                }

                if (pins.Count > 1)
                    frmcons.frmInit(frmM, this, sf, pins, ssuggestednextpin);
                else
                {
                    foreach (ToolStripButton item in frmM.mutuallyExclusiveButtons)
                    {
                        item.Checked = false;
                    }
                    MessageBox.Show("Only one(1) parcel was selected \n\rwhile creating the line, \n\rplease repick you selection.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    frmM.axMap.ClearDrawing(frmM.hndMapSel);
                }
            }
            catch
            {
                foreach (ToolStripButton item in frmM.mutuallyExclusiveButtons)
                {
                    item.Checked = false;
                }
            }
            

            //try
            //{
            //    if (sf.SelectShapes(ext, (double)0, SelectMode.INTERSECTION, ref ShapeIDS))
            //    {
            //        int[] shapes = (int[])ShapeIDS;
            //        if (shapes.Length > 0)
            //        {
            //            for (int i = 0; i < shapes.Length; i++)
            //            {
            //                MapWinGIS.Shape polyshp = sf.get_Shape(Convert.ToInt32(shapes.GetValue(i)));
            //                MapWinGIS.Shape lineshp = frmM.m_newlineshape;
            //                strmotherpin = sf.Table.get_CellValue(pinfldidx, Convert.ToInt32(shapes.GetValue(i))).ToString();
            //                if (strmotherpin != "")
            //                    mothershpincoll.Add(strmotherpin);
            //                mothershpindxcoll.Add(Convert.ToInt32(shapes.GetValue(i)));
            //                pins.Add(strmotherpin,Convert.ToInt32(shapes.GetValue(i)));
                            
            //            }
            //            // Order by values.
            //            // ... Use LINQ to specify sorting by value.
            //            //var items = (from pair in dictionary orderby pair.Value ascending select pair);
            //           // items = (from entry in myDict orderby entry.Value ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);
                        

            //            foreach(string strpin in mothershpincoll)
            //            {
            //                //if(mothershpincoll[0].ToString().Length == 18)
            //                if (strpin.Length == 18)
            //                {
            //                    strtestpin = strpin;
            //                    strtestsect = strpin.Substring(0, 14);
            //                    break;
            //                }
            //            }

            //            if (strtestsect != "")
            //            {
            //                sdbaselastpin = GetDBaseLastPin(strtestpin, dsnOLEDB);
            //                sshapelastpin = GetShapeLastPIN(strtestpin, sf);

            //                if(Convert.ToInt32(sdbaselastpin.Substring(15,3)) > Convert.ToInt32(sshapelastpin.Substring(15,3)))
            //                    ssuggestednextpin = sdbaselastpin.Substring(0, 15) + String.Format("{0:D3}", Convert.ToInt32(sdbaselastpin.Substring(15, 3)) + 1);
            //                else
            //                    ssuggestednextpin = sshapelastpin.Substring(0, 15) + String.Format("{0:D3}", Convert.ToInt32(sshapelastpin.Substring(15, 3)) + 1);
            //            }

            //            if (frmcons == null)
            //                frmcons = new frmConsLand();

            //            frmcons.frmInit(frmM, this, sf ,pins);
            //        }
            //    }
            //}
            //catch
            //{
            //}
        }

        public string returnValue(string sQuery, string sconn, string dbasetable)
        {
            string strval = String.Empty;
            OleDbConnection oOleDbConnection = new OleDbConnection(sconn);
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                OleDbCommand cmd = new OleDbCommand(sQuery, oOleDbConnection);
                OleDbDataAdapter newDataAdapter = new OleDbDataAdapter(cmd);
                oOleDbConnection.Open();
                OleDbDataReader newDataReaders = cmd.ExecuteReader();

                if (newDataReaders.HasRows == true)
                {
                    newDataReaders.Read();
                    strval = newDataReaders[0].ToString();
                }
                newDataReaders.Close();
                oOleDbConnection.Close();
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                return strval;
            }
            catch (Exception err)
            {
                if (oOleDbConnection.State == ConnectionState.Open)
                    oOleDbConnection.Close();
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                MessageBox.Show("Cannot connect to database!\n\r Error in accessing " + dbasetable + "\n\r" + err.Message, "Error Message");
                return (String)"";
            }
        }


        public void resetLine(string cntrl)
        {
            if (cntrl == "SUBDLAND")
            {
                frmM.m_newlineshape = new MapWinGIS.ShapeClass();
                frmM.axMap.ClearDrawings();
                frmM.m_mapcntrl = "SUBDLAND";
                frmM.axMap.CursorMode = tkCursorMode.cmNone;
                frmM.axMap.SendMouseUp = true;
                frmM.axMap.MapCursor = tkCursor.crsrCross;
                bool boolnewlnshp = frmM.m_newlineshape.Create(ShpfileType.SHP_POLYLINE);
            }
            else if (cntrl == "CONSLAND")
            {
                frmM.m_newlineshape = new MapWinGIS.ShapeClass();
                frmM.axMap.ClearDrawings();
                frmM.m_mapcntrl = "CONSLAND";
                frmM.axMap.CursorMode = tkCursorMode.cmNone;
                frmM.axMap.SendMouseUp = true;
                frmM.axMap.MapCursor = tkCursor.crsrCross;
                bool boolnewlnshp = frmM.m_newlineshape.Create(ShpfileType.SHP_POLYLINE);
            }
        }
        
        /// <summary>
        /// Draws a rubber band line attached to the cursor from _startPt(in Map coord) to ScreenX,ScreenY (in screen coords)
        /// So set _startPt when the user clicks and then call this method when the mouse moves. When you want to stop 
        /// drawing set _startPt and _oldMousePt to null and stop calling this function
        /// </summary>
        /// <param name="ScreenX"></param>
        /// <param name="ScreenY"></param>
        public void UpdateScreen(int ScreenX, int ScreenY)
        {
            double x = 0, y = 0;
            //This checks if we just starting and returns if theres nothing to draw yet
            if (_startPt == null && _oldMousePt == null)
                return;

            //Location of the mouse on screen
            System.Drawing.Point mousePt = new System.Drawing.Point(ScreenX, ScreenY);

            //Convert the stored location into screen coord for drawing because
            //we store it in Map coordinants to allow the user to zoom / pan

            frmM.axMap.ProjToPixel(_startPt.x, _startPt.y, ref x, ref y);
            System.Drawing.Point start = new System.Drawing.Point((int)x, (int)y);
            //Check if the mouse has moved from where it started
            if ((start.X == ScreenX && start.Y == ScreenY) || _oldMousePt == null)
            {
                frmM.axMap.PixelToProj(ScreenX, ScreenY, ref x, ref y);
                _oldMousePt = new MapWinGIS.Point();
                _oldMousePt.x = x;
                _oldMousePt.y = y;
                return;
            }

            frmM.axMap.ProjToPixel(_oldMousePt.x, _oldMousePt.y, ref x, ref y);
            System.Drawing.Point old = new System.Drawing.Point((int)x, (int)y);
            System.Drawing.Rectangle mapRect = new Rectangle(0, 0, frmM.axMap.Width, frmM.axMap.Height);
            //Gets the maps graphics object
            System.Drawing.Graphics g = frmM.axMap.CreateGraphics();
            //Defines a invalidation rectangle where the line was drawn before
            g.SmoothingMode = SmoothingMode.AntiAlias;
            PointF invStartPt = new PointF(Math.Min(start.X, old.X), Math.Min(start.Y, old.Y));
            SizeF invSize = new SizeF(Math.Abs(old.X - start.X), Math.Abs(old.Y - start.Y));
            System.Drawing.RectangleF invRect = new System.Drawing.RectangleF(invStartPt, invSize);
            invRect.Inflate(10F, 10F);
            invRect.Intersect(new Rectangle(0, 0, frmM.axMap.Width, frmM.axMap.Height));

            //Converts the invalidation rectangle into a region and exclude where we are drawing so 
            //it doesnt flicker white
            Region invRegion = new Region(invRect);
            GraphicsPath linePath = CohenSutherland.cohenSutherland(start, mousePt, mapRect);
            if (linePath != null)
            {
                linePath.Widen(_LinePen);
                invRegion.Exclude(linePath);
            }

            //This draws the measure line once before and once after the map invalidates
            //so that it doesn't flicker white for a second. To speed up drawing we clip 
            //the line to the screen bounds so off screen drawind doesn't slow things down
            g.Clip = new Region(new Rectangle(0, 0, frmM.axMap.Width, frmM.axMap.Height));
            g.DrawLine(_LinePen, start, mousePt);
            frmM.axMap.Invalidate(invRegion);
            frmM.axMap.Update();
            g.DrawLine(_LinePen, start, mousePt);

            //Backups the mouses current location
            frmM.axMap.PixelToProj(mousePt.X, mousePt.Y, ref x, ref y);
            _oldMousePt.x = x;
            _oldMousePt.y = y;
        }

        void DrawRubberRect(System.Drawing.Graphics g, System.Drawing.Rectangle mapRect, System.Drawing.Point start,
    System.Drawing.Point mousePt, System.Drawing.Point old)
        {
            //Defines a invalidation rectangle where the line was drawn before
            g.SmoothingMode = SmoothingMode.AntiAlias;
            PointF invStartPt = new PointF(Math.Min(start.X, old.X), Math.Min(start.Y, old.Y));
            SizeF invSize = new SizeF(Math.Abs(old.X - start.X), Math.Abs(old.Y - start.Y));
            System.Drawing.RectangleF invRect = new System.Drawing.RectangleF(invStartPt, invSize);
            invRect.Inflate(20F, 20F);
            invRect.Intersect(new Rectangle(0, 0, frmM.axMap.Width, frmM.axMap.Height));

            //Converts the invalidation rectangle into a region and exclude where we are drawing so 
            //it doesnt flicker white
            Region invRegion = new Region(invRect);
            //GraphicsPath linePath = CohenSutherland.cohenSutherland(start, mousePt, mapRect);
            //if (linePath != null)
            //{
            //    linePath.Widen(_LinePen);
            //    invRegion.Exclude(linePath);
            //}

            //This draws the measure line once before and once after the map invalidates
            //so that it doesn't flicker white for a second. To speed up drawing we clip 
            //the line to the screen bounds so off screen drawind doesn't slow things down
            g.Clip = new Region(new Rectangle(0, 0, frmM.axMap.Width, frmM.axMap.Height));

            System.Drawing.Point newStartPt = new System.Drawing.Point(Math.Min(start.X, mousePt.X), Math.Min(start.Y, mousePt.Y));
            Size newSize = new Size(Math.Abs(mousePt.X - start.X), Math.Abs(mousePt.Y - start.Y));
            System.Drawing.Rectangle newRect = new Rectangle(newStartPt, newSize);

            g.DrawRectangle(_LinePen, newRect);
            frmM.axMap.Invalidate(invRegion);
            frmM.axMap.Update();

            g.DrawRectangle(_LinePen, newRect);
            //g.FillRectangle(Brushes.Blue, newRect);
        }

        public string GetDBaseLastPin(string spin, string dsnOLEDB)
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            string lastpin = String.Empty;

            String sConnString = dsnOLEDB;
            OleDbCommand cmd = new OleDbCommand();
            OleDbConnection oOleDbConnection = new OleDbConnection(sConnString);
            oOleDbConnection.Open();

            StringBuilder sqry = new StringBuilder();
            sqry.Append("select max(land_pin) lastpin ");
            sqry.Append(string.Format("from {0}.faas_land where substr(trim(land_pin),0,14) like '{1}%'",frmM.m_rptdbase,spin.Substring(0,14)));
            if(spin.Length == 18)
                sqry.Append(string.Format(" and substr(trim(land_pin),15,3) != {0}","999"));

            cmd = new OleDbCommand(sqry.ToString(), oOleDbConnection);
            OleDbDataAdapter newDataAdapter = new OleDbDataAdapter(cmd);
            DataSet newDataSet = new DataSet();
            OleDbDataReader newDataReaders = cmd.ExecuteReader();

            if (newDataReaders.HasRows)
            {
                while (newDataReaders.Read())
                {
                    if (!DBNull.Value.Equals(newDataReaders["LASTPIN"]))
                    {
                        lastpin = (String)newDataReaders["LASTPIN"].ToString();
                    }
                    else
                    {
                        lastpin = spin;
                    }
                }
                newDataReaders.Close();
                oOleDbConnection.Close();
                return lastpin;
            }
            else
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                return lastpin;
            }
        }

        public string GetPinFromSectBndry(Extents ext,string strLayer)
        {
            string val = String.Empty;
            Shapefile sf = new Shapefile();
            int lyr = -1;

            for (int i = 0; i < frmM.legend1.Layers.Count; i++)
            {
                if (frmM.legend1.Map.get_LayerName(i).ToUpper() == strLayer)
                {
                    sf = (Shapefile)frmM.axMap.get_GetObject(i);
                    lyr = i;
                    break;
                }
            }
            if (lyr != -1)
            {
                object ShapeIDS = new object();
                if (sf.SelectShapes(ext, (double)0, SelectMode.INTERSECTION, ref ShapeIDS))
                {
                    int[] shapes = (int[])ShapeIDS;
                    if (shapes.Length != 0)
                    {
                        int fldidx = -1;
                        fldidx = sf.Table.get_FieldIndexByName("PIN");
                        if(fldidx != -1)
                        {
                            val = sf.get_CellValue(fldidx, Convert.ToInt32(shapes.GetValue(0).ToString())).ToString();
                            //val = GetDBaseLastPin(sf.get_CellValue(fldidx, Convert.ToInt32(shapes.GetValue(0).ToString())).ToString(), dsnOLEDB);
                        }
                    }
                }
            }
            return val;
        }

        public string GetShapeLastPIN(string sval, Shapefile sf)
        {
            string lastpin = String.Empty;
            string error2 = "";
            object result2 = null;
            StringBuilder sqry = new StringBuilder();
            sqry.Append(string.Format("[BRGY] = \"{0}\"", sval.Substring(7, 3)));
            sqry.Append(string.Format(" AND [SECT] = \"{0}\"", sval.Substring(11, 3)));
            sqry.Append(string.Format(" AND [PRCL] <> \"{0}\"", "999"));
            if (sf.Table.Query(sqry.ToString(), ref result2, ref error2))
            {
                int[] shapes2 = (int[])result2;
                ArrayList prclColl = new ArrayList();
                int intprcl;
                for (int iii = 0; iii < shapes2.Length; iii++)
                {
                    int fidx = -1;
                    fidx = sf.Table.get_FieldIndexByName("PRCL");
                    if (fidx != -1)
                    {
                        prclColl.Add(sf.get_CellValue(fidx, Convert.ToInt32(shapes2.GetValue(iii).ToString())).ToString());
                    }
                }
                prclColl.Sort();
                prclColl.Reverse();
                if (int.TryParse(prclColl[0].ToString(), out intprcl))
                    lastpin = sval.Substring(0, 15) + String.Format("{0:D3}", Convert.ToInt32(prclColl[0].ToString()));
            }
            else
            {
                lastpin = sval;
            }
            return lastpin;
        }

        public bool validatePayment(string strconn, string spin)
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                Boolean returnVal = true;
                String sConnString = strconn;
                OleDbCommand cmd = new OleDbCommand();
                OleDbConnection oOleDbConnection = new OleDbConnection(sConnString);
                oOleDbConnection.Open();
                
                StringBuilder squery = new StringBuilder();
                squery.Append("SELECT trim(x0.pin) pin,x0.payment_type,x0.payment_period,x0.tax_year ");
                squery.Append(String.Format("FROM {0}.payment_hist x0, {0}.own_names x0a ",frmM.m_rptdbase));
                squery.Append("WHERE (trim(x0.own_code) = trim(x0a.own_code)) ");
                squery.Append("AND ((x0.tax_year = (SELECT MAX(x1.tax_year) ");
                squery.Append(String.Format("FROM {0}.payment_hist x1 ", frmM.m_rptdbase));
                squery.Append("WHERE (trim(x0.pin) = trim(x1.pin)))) ");
                squery.Append("AND (x0.payment_period = (SELECT MAX(x2.payment_period) ");
                squery.Append(String.Format("FROM {0}.payment_hist x2 ", frmM.m_rptdbase));
                squery.Append("WHERE ((trim(x0.pin) = trim(x2.pin)) AND (trim(x2.tax_year) = trim(x0.tax_year)))))) ");
                squery.Append(String.Format("AND trim(x0.pin) = '{0}'", spin));
                
                cmd = new OleDbCommand(squery.ToString(), oOleDbConnection);
                OleDbDataAdapter newDataAdapter = new OleDbDataAdapter(cmd);
                DataSet newDataSet = new DataSet();
                OleDbDataReader newDataReaders = cmd.ExecuteReader();
                int curQtr = Convert.ToInt16(DateTime.Now.Month) / 3;

                if (newDataReaders.HasRows)
                {
                    String strVal = "";
                    String payPeriod = "";
                    String payType = "";
                    while (newDataReaders.Read())
                    {
                        if (!DBNull.Value.Equals(newDataReaders["TAX_YEAR"]))
                        {
                            strVal = (String)newDataReaders["TAX_YEAR"].ToString();
                        }

                        if (Convert.ToInt16(strVal) < Convert.ToInt16(DateTime.Now.Year))
                        {
                            returnVal = false;
                        }
                        else
                        {
                            if (!DBNull.Value.Equals(newDataReaders["PAYMENT_TYPE"]))
                            {
                                payType = (String)newDataReaders["PAYMENT_TYPE"].ToString();
                            }
                            if (payType != "")
                            {
                                if (payType == "I")
                                {
                                    if (!DBNull.Value.Equals(newDataReaders["PAYMENT_PERIOD"]))
                                    {
                                        payPeriod = (String)newDataReaders["PAYMENT_PERIOD"].ToString();
                                    }
                                    if (payPeriod == "")
                                    {
                                        returnVal = false;
                                    }
                                    if (Convert.ToInt16(payPeriod) < curQtr)
                                    {
                                        returnVal = false;
                                    }
                                }
                            }
                            else
                            {
                                returnVal = false;
                            }
                        }
                    }
                    if (strVal == "")
                    {
                        returnVal = false;
                    }
                }
                else
                {
                    returnVal = false;
                }
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                newDataReaders.Close();
                oOleDbConnection.Close();
                return returnVal;

            }
            catch (Exception e)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                MessageBox.Show(e.Message);
                return false;
            }
        }

        public void updateDBASE(String sConnString, String sQry)
        {
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                OleDbConnection oOleDbConnection = new OleDbConnection(sConnString);
                OleDbCommand cmd = new OleDbCommand(sQry, oOleDbConnection);
                oOleDbConnection.Open();
                cmd.ExecuteNonQuery();
                oOleDbConnection.Close();
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch (OleDbException err)
            {
                int errcode = err.ErrorCode;
                switch (errcode)
                {
                    case -2147217865:
                        //err.
                        break;
                    default:
                        MessageBox.Show(err.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
                   
            }
        }


        /// <summary>
        /// audit trail for the GIS database, gis_a_trail_parcel table
        /// </summary>
        /// <param name="sUserID"></param>
        /// <param name="sModule"></param>
        /// <param name="sAffTable"></param>
        /// <param name="sComputer"></param>
        /// <param name="sDetails"></param>
        /// <param name="sConnString"></param>
        public void AddATrail(String sUserID, String sModule, String sAffTable, String sComputer, String sDetails, String sConnString)
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                OleDbConnection oOleDbConnection = new OleDbConnection(sConnString);
                StringBuilder sqry = new StringBuilder();
                sqry.Append("insert into gis_a_trail_parcel values(");
                sqry.Append(String.Format("'{0}',", sUserID));
                sqry.Append(String.Format("{0},", "SYSDATE"));
                sqry.Append(String.Format("'{0}',", sModule));
                sqry.Append(String.Format("'{0}',", sAffTable));
                sqry.Append(String.Format("'{0}',", sComputer));
                sqry.Append(String.Format("'{0}'", sDetails));
                sqry.Append(")");
                //String mystring = "insert into gis_a_trail_parcel values('" + sUserID + "',SYSDATE,'" + sModule + "','" + sAffTable + "','" + sComputer + "','" + sDetails + "')";
                OleDbCommand cmd = new OleDbCommand(sqry.ToString(), oOleDbConnection);
                oOleDbConnection.Open();
                cmd.ExecuteNonQuery();
                oOleDbConnection.Close();
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        }

        public String CheckInStr(String strA)
        {
            String strB = (String)"";
            if (strA.IndexOf("'") != -1)
            {
                strB = "'" + strA + "'";
            }
            else
            {
                strB = "'" + strA.Replace("'", "''") + "'";

            }
            return strB;
        }

        public Color UIntToColor(uint color)
        {
            byte a = (byte)(color >> 24);
            byte r = (byte)(color >> 16);
            byte g = (byte)(color >> 8);
            byte b = (byte)(color >> 0);
            return Color.FromArgb(a, r, g, b);
        }

        public DataTable populateDataTable2(string conn, string sqry, string dbasetable)
        {
            DataTable dataTable = new DataTable();
            OleDbConnection oOleDbConnection = new OleDbConnection(conn);
            oOleDbConnection.Open();
            try
            {
                OleDbCommand cmd = new OleDbCommand(sqry, oOleDbConnection);
                OleDbDataReader newDataReaders = cmd.ExecuteReader();
                if (newDataReaders.HasRows)
                    dataTable.Load(newDataReaders);

                newDataReaders.Close();
                oOleDbConnection.Close();
            }
            catch(Exception err)
            {
                MessageBox.Show("Cannot connect to database!\n\r Error in accessing " + dbasetable + "\n\r" + err.Message, "Error Message");
                return dataTable;
            }
            return dataTable;
        }

        public DataTable populateDataTable(Shapefile sf)
        {
            // Variables related to the C# data table 
            DataTable dataTable = new DataTable();
            DataColumn dataColumn;
            DataRow myDataRow;
            Field myField;

            dataColumn = new DataColumn();
            dataColumn.DataType = typeof(int);
            dataColumn.ColumnName = "SHAPE__ID";
            dataColumn.AutoIncrement = true;
            dataColumn.Unique = true;

            dataTable.Columns.Add(dataColumn);
            // Add the columns to the table based on the fields
            for (int fld = 0; fld < sf.NumFields; fld++)
            {
                dataColumn = new DataColumn();
                myField = sf.get_Field(fld);

                // Use the existing field name in order to give the column a name
                dataColumn.ColumnName = myField.Name;

                // Set up columns with different data types depending on the type specified in the shapefile
                if (myField.Type == MapWinGIS.FieldType.DOUBLE_FIELD)
                {
                    dataColumn.DataType = typeof(double);
                }
                else if (myField.Type == MapWinGIS.FieldType.INTEGER_FIELD)
                {
                    dataColumn.DataType = typeof(int);
                }
                else
                {
                    dataColumn.DataType = typeof(string);
                }

                // Add each column
                dataTable.Columns.Add(dataColumn);
            }
            // Add the rows to the table based on the cell values
            for (int shp = 0; shp < sf.NumShapes; shp++)
            {
                myDataRow = dataTable.NewRow();

                // Each row has information for all the columns.
                for (int fld = 0; fld < sf.NumFields; fld++)
                {
                    myField = sf.get_Field(fld);

                    // This just explicitly casts from the object values into the correct
                    // field type for the Data Grid
                    if (myField.Type == MapWinGIS.FieldType.DOUBLE_FIELD)
                    {
                        if (sf.get_CellValue(fld, shp) != null)
                            myDataRow[fld + 1] = DBNull.Value == sf.get_CellValue(fld, shp) ? 0 : (double)sf.get_CellValue(fld, shp);
                        else
                            myDataRow[fld + 1] = 0;
                    }
                    else if (myField.Type == MapWinGIS.FieldType.INTEGER_FIELD)
                    {
                        if (sf.get_CellValue(fld, shp) != null)
                            myDataRow[fld + 1] = DBNull.Value == sf.get_CellValue(fld, shp) ? 0 : (int)sf.get_CellValue(fld, shp);
                        else
                            myDataRow[fld + 1] = 0;
                    }
                    else
                    {
                        if (sf.get_CellValue(fld, shp) != null)
                            myDataRow[fld + 1] = (string)sf.get_CellValue(fld, shp);
                        else
                            myDataRow[fld + 1] = "";
                    }

                }
                // Once we add the values to a row, add the rows to the table.
                dataTable.Rows.Add(myDataRow);
            }

            return dataTable;
        }

        public Shapefile loadlandmarkpt(string strqry, string strconn, frmMain m_frmM)
        {
            Shapefile sf = new MapWinGIS.Shapefile();
            try
            {
                
                string sfname = String.Empty;

                if (System.IO.File.Exists("landmark_pt.shp"))
                    System.IO.File.Delete("landmark_pt.shp");
                if (System.IO.File.Exists("landmark_pt.shx"))
                    System.IO.File.Delete("landmark_pt.shx");
                if (System.IO.File.Exists("landmark_pt.prj"))
                    System.IO.File.Delete("landmark_pt.prj");
                if (System.IO.File.Exists("landmark_pt.dbf"))
                    System.IO.File.Delete("landmark_pt.dbf");

                ////Create directoy
                //if (!Directory.Exists("landmarkpt"))
                //    Directory.CreateDirectory("landmarkpt");

                sfname = "landmark_pt.shp";

                ////shp file
                //if (File.Exists("landmark_pt.shp"))
                //{
                //    if (File.Exists("landmarkpt\\landmark_pt.shp"))
                //    {
                //        File.Delete("landmarkpt\\landmark_pt.shp");
                //        File.Move("landmark_pt.shp","landmarkpt\\landmark_pt.shp");
                //    }
                //    else
                //        File.Move("landmark_pt.shp", "landmarkpt\\landmark_pt.shp");
                //}
                //if (File.Exists("landmark_pt1.shp"))
                //{
                //    if (File.Exists("landmarkpt\\landmark_pt1.shp"))
                //    {
                //        File.Delete("landmarkpt\\landmark_pt1.shp");
                //        File.Move("landmark_pt1.shp", "landmarkpt\\landmark_pt1.shp");
                //    }
                //    else
                //        File.Move("landmark_pt1.shp", "landmarkpt\\landmark_pt1.shp");
                //}

                ////shx file
                //if (File.Exists("landmark_pt.shx"))
                //{
                //    if (File.Exists("landmarkpt\\landmark_pt.shx"))
                //    {
                //        File.Delete("landmarkpt\\landmark_pt.shx");
                //        File.Move("landmark_pt.shx", "landmarkpt\\landmark_pt.shx");
                //    }
                //    else
                //        File.Move("landmark_pt.shx", "landmarkpt\\landmark_pt.shx");
                //}
                //if (File.Exists("landmark_pt1.shx"))
                //{
                //    if (File.Exists("landmarkpt\\landmark_pt1.shx"))
                //    {
                //        File.Delete("landmarkpt\\landmark_pt1.shx");
                //        File.Move("landmark_pt1.shx", "landmarkpt\\landmark_pt1.shx");
                //    }
                //    else
                //        File.Move("landmark_pt1.shx", "landmarkpt\\landmark_pt1.shx");
                //}

                //try
                //{
                //    //dbf file
                //    if (File.Exists("landmark_pt.dbf"))
                //    {
                //        if (File.Exists("landmarkpt\\landmark_pt.dbf"))
                //        {
                //            File.Delete("landmarkpt\\landmark_pt.dbf");
                //            File.Move("landmark_pt.dbf", "landmarkpt\\landmark_pt.dbf");
                //        }
                //        else
                //            File.Move("landmark_pt.dbf", "landmarkpt\\landmark_pt.dbf");
                //    }
                //}
                //catch
                //{
                //    if (File.Exists("landmark_pt1.dbf"))
                //        File.Move("landmark_pt1.dbf", "landmarkpt\\landmark_pt1.dbf");
                //    sfname = "landmark_pt1.shp";
                //}

                ////prj file
                //if (File.Exists("landmark_pt.prj"))
                //{
                //    if (File.Exists("landmarkpt\\landmark_pt.prj"))
                //    {
                //        File.Delete("landmarkpt\\landmark_pt.prj");
                //        File.Move("landmark_pt.prj", "landmarkpt\\landmark_pt.prj");
                //    }
                //    else
                //        File.Move("landmark_pt.prj", "landmarkpt\\landmark_pt.prj");
                //}
                //if (File.Exists("landmark_pt1.prj"))
                //{
                //    if (File.Exists("landmarkpt\\landmark_pt1.prj"))
                //    {
                //        File.Delete("landmarkpt\\landmark_pt1.prj");
                //        File.Move("landmark_pt1.prj", "landmarkpt\\landmark_pt1.prj");
                //    }
                //    else
                //        File.Move("landmark_pt1.prj", "landmarkpt\\landmark_pt1.prj");
                //}

                bool result = sf.CreateNew(sfname, MapWinGIS.ShpfileType.SHP_POINT);

                int intfld = 0;
                if (result == true)
                    result = sf.StartEditingShapes(true, null);

                //create fieldnames
                MapWinGIS.Field fld = new MapWinGIS.FieldClass();
                fld.Name = "LM_ID";
                fld.Width = 10;
                fld.Type = MapWinGIS.FieldType.INTEGER_FIELD;
                sf.EditInsertField(fld, ref intfld, null);

                intfld += 1;
                fld = new MapWinGIS.FieldClass();
                fld.Name = "LM_NAME";
                fld.Width = 100;
                fld.Type = MapWinGIS.FieldType.STRING_FIELD;
                sf.EditInsertField(fld, ref intfld, null);

                intfld += 1;
                fld = new MapWinGIS.FieldClass();
                fld.Name = "LM_DESC";
                fld.Width = 200;
                fld.Type = MapWinGIS.FieldType.STRING_FIELD;
                sf.EditInsertField(fld, ref intfld, null);

                intfld += 1;
                fld = new MapWinGIS.FieldClass();
                fld.Name = "LM_TYPE";
                fld.Width = 100;
                fld.Type = MapWinGIS.FieldType.STRING_FIELD;
                sf.EditInsertField(fld, ref intfld, null);

                intfld += 1;
                fld = new MapWinGIS.FieldClass();
                fld.Name = "CATEGORY";
                fld.Width = 100;
                fld.Type = MapWinGIS.FieldType.STRING_FIELD;
                sf.EditInsertField(fld, ref intfld, null);

                intfld += 1;
                fld = new MapWinGIS.FieldClass();
                fld.Name = "X";
                fld.Width = 10;
                fld.Precision = 6;
                fld.Type = MapWinGIS.FieldType.DOUBLE_FIELD;
                sf.EditInsertField(fld, ref intfld, null);

                intfld += 1;
                fld = new MapWinGIS.FieldClass();
                fld.Name = "Y";
                fld.Width = 10;
                fld.Precision = 6;
                fld.Type = MapWinGIS.FieldType.DOUBLE_FIELD;
                sf.EditInsertField(fld, ref intfld, null);

                OleDbConnection oledbconn = new OleDbConnection(frmM.m_strconn);
                OleDbCommand cmd = new OleDbCommand(strqry, oledbconn);
                OleDbDataAdapter newDataAdapter = new OleDbDataAdapter(cmd);
                DataSet newDataSet = new DataSet();
                oledbconn.Open();
                OleDbDataReader newDataReaders = cmd.ExecuteReader();

                int intid = 0;
                string strnm = String.Empty;
                string strdesc = String.Empty;
                string strtype = String.Empty;
                string strcat = String.Empty;
                double dblx = 0.0;
                double dbly = 0.0;
                int ptref = 0;
                bool boolSuccess = false;


                if (newDataReaders.HasRows == true)
                {
                    while (newDataReaders.Read())     
                    {
                        MapWinGIS.Point shpt = new MapWinGIS.Point();
                        if (!DBNull.Value.Equals(newDataReaders["ID"]))
                            intid = Convert.ToInt32(newDataReaders["ID"].ToString());
                        if (!DBNull.Value.Equals(newDataReaders["LM_NAME"]))
                            strnm = newDataReaders["LM_NAME"].ToString().Trim().ToUpper();
                        if (!DBNull.Value.Equals(newDataReaders["LM_DESC"]))
                            strdesc = newDataReaders["LM_DESC"].ToString().Trim().ToUpper();
                        if (!DBNull.Value.Equals(newDataReaders["LM_TYPE"]))
                            strtype = newDataReaders["LM_TYPE"].ToString().Trim().ToUpper();
                        if (!DBNull.Value.Equals(newDataReaders["LM_CATEGORY"]))
                            strcat = newDataReaders["LM_CATEGORY"].ToString().Trim().ToUpper();
                        if (!DBNull.Value.Equals(newDataReaders["X"]))
                            dblx = Convert.ToDouble(newDataReaders["X"].ToString());
                        if (!DBNull.Value.Equals(newDataReaders["Y"]))
                            dbly = Convert.ToDouble(newDataReaders["Y"].ToString());

                        shpt.x = dblx;
                        shpt.y = dbly;

                        MapWinGIS.Shape sh1 = new MapWinGIS.ShapeClass();
                        boolSuccess = sh1.Create(MapWinGIS.ShpfileType.SHP_POINT);
                        ptref = ptref + 1;

                        boolSuccess = sh1.InsertPoint(shpt, ref ptref);
                        boolSuccess = sf.EditInsertShape(sh1, ref ptref);

                        bool boolCellAdd;
                        boolCellAdd = sf.EditCellValue(0, ptref, intid);
                        boolCellAdd = sf.EditCellValue(1, ptref, strnm);
                        boolCellAdd = sf.EditCellValue(2, ptref, strdesc);
                        boolCellAdd = sf.EditCellValue(3, ptref, strtype);
                        boolCellAdd = sf.EditCellValue(4, ptref, strcat);
                        boolCellAdd = sf.EditCellValue(5, ptref, shpt.x);
                        boolCellAdd = sf.EditCellValue(6, ptref, shpt.y);

                    }
                }

                boolSuccess = sf.StopEditingShapes(true, true, null);
                sf.SaveAs(m_frmM.m_shapelandmarklocation + @"\" +  sf.Filename, null);

                sf.Open(sf.Filename, null);

                //setting the categories for unique values and gradient legend
                sf = legendLandmarks(m_frmM, sf, Convert.ToUInt32(ColorTranslator.ToOle(Color.LightGray)), Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray)), true, (double)1, (double)1);

                //int iFldInx = -1;
                //Color cfontcolor = Color.White;
                //int intfontsize = 8;
                //iFldInx = sf.Table.get_FieldIndexByName("LM_NAME");
                //if (sf != null)
                //    sf = setlabel2(sf, iFldInx, "ARIAL", cfontcolor, intfontsize, true, false, tkLabelFrameType.lfRoundedRectangle, Convert.ToUInt32(ColorTranslator.ToOle(Color.Chocolate)), Convert.ToUInt32(ColorTranslator.ToOle(Color.SandyBrown)), true, false);

                //adding the shapefile as layer
                frmM.hndMap = frmM.legend1.Layers.Add(sf, true);
                frmM.legend1.Layers.MoveLayer(frmM.hndMap, frmM.m_groupvector, frmM.legend1.Groups[frmM.m_groupvector].LayerCount);
                frmM.legend1.Map.set_LayerName(frmM.hndMap, "LANDMARKS");
                frmM.legend1.Layers.ItemByHandle(frmM.hndMap).Expanded = true;
                frmM.legend1.Layers.ItemByHandle(frmM.hndMap).Refresh();

                frmM.hndMapSel = frmM.hndMap;
                frmM.m_sellayername = "LANDMARKS";
                frmM.legend1.Map.Redraw();

                newDataReaders.Close();
                oledbconn.Close();
                 
                //if(sf.EditingTable)
                //    sf.StopEditingTable(false,null);
                //if(sf.EditingShapes)
                //    sf.StopEditingShapes(false,false,null);

                //sf.Close();
                //sf = null;
                return sf;

            }
            catch
            {
                //MessageBox.Show(err.Message);
                return null;
            }
        }

        public Shapefile legendLandmarks(frmMain m_frmM, Shapefile sf, UInt32 trnsColor, UInt32 trnsColor2, bool useTrans, double picXscale, double picYscale)
        {
            Cursor.Current = Cursors.WaitCursor;
            if(sf!=null)
            {
                if(sf.Table!=null)
                {
                    Table sftable = sf.Table;
                    int fldidx = -1;
                    //int pinfldidx = -1;
                    string strpinval = String.Empty;

                    //to do set the fieldnames as dymanic
                    fldidx = sftable.get_FieldIndexByName("CATEGORY");
                    //bool result = sf.StartEditingShapes(true, null);

                    sf.DefaultDrawingOptions.FillVisible = true;
                    sf.Categories.Clear();

                    MapWinGIS.Image img = new MapWinGIS.Image();

                    sf.DefaultDrawingOptions.PictureScaleX = 1;
                    sf.DefaultDrawingOptions.PictureScaleY = 1;
                    sf.DefaultDrawingOptions.PointRotation = (double)0;
                    sf.DefaultDrawingOptions.LineVisible = false;

                    //generates a temporary category for further updates
                    if(fldidx >= 0)
                        sf.Categories.Generate(fldidx, tkClassificationType.ctUniqueValues, 0);

                    bool boolcommercial = false;
                    bool booleducational = false;
                    bool boolfinancial = false;
                    bool boolfire = false;
                    bool boolgas = false;
                    bool boolgovernment = false;
                    bool boolindustrial = false;
                    bool boolmedical = false;
                    bool boolothers = false;
                    bool boolpolice = false;
                    bool boolrecreational = false;
                    bool boolreligious = false;
                    bool boolsubdivision = false;
                    bool booltransportation = false;
                    bool booltelecommunication = false;

                    //setting the category color
                    Utils myUtil = new Utils();
                    for (int i = 0; i < sf.Categories.Count; i++)
                    {
                        ShapefileCategory cat = sf.Categories.get_Item(i);

                        img = new MapWinGIS.Image();
                        //img.TransparencyColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.LightGray));
                        //img.TransparencyColor2 = Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray));
                        img.UseTransparencyColor = false;
                        //cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;

                        if (cat.Name == "COMMERCIAL ENTITIES")
                        {
                            boolcommercial = true;
                            //img.Picture = IconConverter.GetIPictureDispFromImage(Properties.Resources.commercial);
                            //img.Picture = myUtil.hBitmapToPicture(Properties.Resources.commercial.GetHbitmap().ToInt32());
                            if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_commercial.png"))
                            {
                                img.Open(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_commercial.png", ImageType.USE_FILE_EXTENSION, false, null);
                                cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                                cat.DrawingOptions.Picture = img;
                            }
                            else
                            {
                                cat.DrawingOptions.PointShape = tkPointShapeType.ptShapeFlag;
                                cat.DrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Red));
                                cat.DrawingOptions.PointSidesCount = 5;
                                cat.DrawingOptions.PointSidesRatio = (float)0.3;
                                cat.DrawingOptions.PointSize = (float)15;
                                cat.DrawingOptions.PointRotation = (double)0;
                                cat.DrawingOptions.LineVisible = true;
                                cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
                            }

                        }
                        else if (cat.Name == "EDUCATIONAL ENTITIES")
                        {
                            booleducational = true;
                            //img.Picture = IconConverter.GetIPictureDispFromImage(Properties.Resources.educational);
                            //img.Picture = myUtil.hBitmapToPicture(Properties.Resources.educational.GetHbitmap().ToInt32());
                            if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_educational.png"))
                            {
                                img.Open(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_educational.png", ImageType.USE_FILE_EXTENSION, false, null);
                                cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                                cat.DrawingOptions.Picture = img;
                            }
                            else
                            {
                                cat.DrawingOptions.PointShape = tkPointShapeType.ptShapeFlag;
                                cat.DrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Blue));
                                cat.DrawingOptions.PointSidesCount = 5;
                                cat.DrawingOptions.PointSidesRatio = (float)0.3;
                                cat.DrawingOptions.PointSize = (float)15;
                                cat.DrawingOptions.PointRotation = (double)0;
                                cat.DrawingOptions.LineVisible = true;
                                cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
                            }
                        }
                        else if (cat.Name == "FINANCIAL ENTITIES")
                        {
                            boolfinancial= true;
                            //img.Picture = IconConverter.GetIPictureDispFromImage(Properties.Resources.financial1);
                            //img.Picture = myUtil.hBitmapToPicture(Properties.Resources.financial1.GetHbitmap().ToInt32());
                            if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_financial.png"))
                            {
                                img.Open(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_financial.png", ImageType.USE_FILE_EXTENSION, false, null);
                                cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                                cat.DrawingOptions.Picture = img;
                            }
                            else
                            {
                                cat.DrawingOptions.PointShape = tkPointShapeType.ptShapeFlag;
                                cat.DrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Brown));
                                cat.DrawingOptions.PointSidesCount = 5;
                                cat.DrawingOptions.PointSidesRatio = (float)0.3;
                                cat.DrawingOptions.PointSize = (float)15;
                                cat.DrawingOptions.PointRotation = (double)0;
                                cat.DrawingOptions.LineVisible = true;
                                cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
                            }
                        }
                        else if (cat.Name == "FIRE STATION")
                        {
                            boolfire = true;
                            //img.Picture = IconConverter.GetIPictureDispFromImage(Properties.Resources.firestation);
                            //img.Picture = myUtil.hBitmapToPicture(Properties.Resources.firestation.GetHbitmap().ToInt32());
                            if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_firestation.png"))
                            {
                                img.Open(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_firestation.png", ImageType.USE_FILE_EXTENSION, false, null);
                                cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                                cat.DrawingOptions.Picture = img;
                            }
                            else
                            {
                                cat.DrawingOptions.PointShape = tkPointShapeType.ptShapeFlag;
                                cat.DrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Salmon));
                                cat.DrawingOptions.PointSidesCount = 5;
                                cat.DrawingOptions.PointSidesRatio = (float)0.3;
                                cat.DrawingOptions.PointSize = (float)15;
                                cat.DrawingOptions.PointRotation = (double)0;
                                cat.DrawingOptions.LineVisible = true;
                                cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
                            }
                        }
                        else if (cat.Name == "GAS STATION")
                        {
                            boolgas = true;
                            //img.Picture = IconConverter.GetIPictureDispFromImage(Properties.Resources.gasstation);
                            //img.Picture = myUtil.hBitmapToPicture(Properties.Resources.gasstation.GetHbitmap().ToInt32());
                            if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_gasstation.png"))
                            {
                                img.Open(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_gasstation.png", ImageType.USE_FILE_EXTENSION, false, null);
                                cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                                cat.DrawingOptions.Picture = img;
                            }
                            else
                            {
                                cat.DrawingOptions.PointShape = tkPointShapeType.ptShapeFlag;
                                cat.DrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Silver));
                                cat.DrawingOptions.PointSidesCount = 5;
                                cat.DrawingOptions.PointSidesRatio = (float)0.3;
                                cat.DrawingOptions.PointSize = (float)15;
                                cat.DrawingOptions.PointRotation = (double)0;
                                cat.DrawingOptions.LineVisible = true;
                                cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
                            }
                        }
                        else if (cat.Name == "GOVERNMENT ENTITIES")
                        {
                            boolgovernment = true;
                            //img.Picture = IconConverter.GetIPictureDispFromImage(Properties.Resources.government);
                            //img.Picture = myUtil.hBitmapToPicture(Properties.Resources.government.GetHbitmap().ToInt32());
                            if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_government.png"))
                            {
                                img.Open(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_government.png", ImageType.USE_FILE_EXTENSION, false, null);
                                cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                                cat.DrawingOptions.Picture = img;
                            }
                            else
                            {
                                cat.DrawingOptions.PointShape = tkPointShapeType.ptShapeFlag;
                                cat.DrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Tan));
                                cat.DrawingOptions.PointSidesCount = 5;
                                cat.DrawingOptions.PointSidesRatio = (float)0.3;
                                cat.DrawingOptions.PointSize = (float)15;
                                cat.DrawingOptions.PointRotation = (double)0;
                                cat.DrawingOptions.LineVisible = true;
                                cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
                            }
                        }
                        else if (cat.Name == "INDUSTRIAL ENTITIES")
                        {
                            boolindustrial = true;
                            //img.Picture = IconConverter.GetIPictureDispFromImage(Properties.Resources.industrial);
                            //img.Picture = myUtil.hBitmapToPicture(Properties.Resources.industrial.GetHbitmap().ToInt32());
                            if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_industrial.png"))
                            {
                                img.Open(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_industrial.png", ImageType.USE_FILE_EXTENSION, false, null);
                                cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                                cat.DrawingOptions.Picture = img;
                            }
                            else
                            {
                                cat.DrawingOptions.PointShape = tkPointShapeType.ptShapeFlag;
                                cat.DrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Purple));
                                cat.DrawingOptions.PointSidesCount = 5;
                                cat.DrawingOptions.PointSidesRatio = (float)0.3;
                                cat.DrawingOptions.PointSize = (float)15;
                                cat.DrawingOptions.PointRotation = (double)0;
                                cat.DrawingOptions.LineVisible = true;
                                cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
                            }
                        }
                        else if (cat.Name == "MEDICAL ENTITIES")
                        {
                            boolmedical = true;
                            //img.Picture = IconConverter.GetIPictureDispFromImage(Properties.Resources.medical);
                            //img.Picture = myUtil.hBitmapToPicture(Properties.Resources.medical.GetHbitmap().ToInt32());
                            if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_medical.png"))
                            {
                                img.Open(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_medical.png", ImageType.USE_FILE_EXTENSION, false, null);
                                cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                                cat.DrawingOptions.Picture = img;
                            }
                            else
                            {
                                cat.DrawingOptions.PointShape = tkPointShapeType.ptShapeFlag;
                                cat.DrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Yellow));
                                cat.DrawingOptions.PointSidesCount = 5;
                                cat.DrawingOptions.PointSidesRatio = (float)0.3;
                                cat.DrawingOptions.PointSize = (float)15;
                                cat.DrawingOptions.PointRotation = (double)0;
                                cat.DrawingOptions.LineVisible = true;
                                cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
                            }
                        }
                        else if (cat.Name == "OTHERS")
                        {
                            boolothers = true;
                            //img.Picture = IconConverter.GetIPictureDispFromImage(Properties.Resources.others);
                            //img.Picture = myUtil.hBitmapToPicture(Properties.Resources.others.GetHbitmap().ToInt32());
                            if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_others.png"))
                            {
                                img.Open(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_others.png", ImageType.USE_FILE_EXTENSION, false, null);
                                cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                                cat.DrawingOptions.Picture = img;
                            }
                            else
                            {
                                cat.DrawingOptions.PointShape = tkPointShapeType.ptShapeFlag;
                                cat.DrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Orange));
                                cat.DrawingOptions.PointSidesCount = 5;
                                cat.DrawingOptions.PointSidesRatio = (float)0.3;
                                cat.DrawingOptions.PointSize = (float)15;
                                cat.DrawingOptions.PointRotation = (double)0;
                                cat.DrawingOptions.LineVisible = true;
                                cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
                            }
                        }
                        else if (cat.Name == "POLICE STATION")
                        {
                            boolpolice = true;
                            //img.Picture = IconConverter.GetIPictureDispFromImage(Properties.Resources.police);
                            //img.Picture = myUtil.hBitmapToPicture(Properties.Resources.police.GetHbitmap().ToInt32());
                            if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_police.png"))
                            {
                                img.Open(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_police.png", ImageType.USE_FILE_EXTENSION, false, null);
                                cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                                cat.DrawingOptions.Picture = img;
                            }
                            else
                            {
                                cat.DrawingOptions.PointShape = tkPointShapeType.ptShapeFlag;
                                cat.DrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Brown));
                                cat.DrawingOptions.PointSidesCount = 5;
                                cat.DrawingOptions.PointSidesRatio = (float)0.3;
                                cat.DrawingOptions.PointSize = (float)15;
                                cat.DrawingOptions.PointRotation = (double)0;
                                cat.DrawingOptions.LineVisible = true;
                                cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
                            }
                        }
                        else if (cat.Name == "RECREATIONAL ENTITIES")
                        {
                            boolrecreational = true;
                            //img.Picture = IconConverter.GetIPictureDispFromImage(Properties.Resources.recreational);
                            //img.Picture = myUtil.hBitmapToPicture(Properties.Resources.recreational.GetHbitmap().ToInt32());
                            if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_recreational.png"))
                            {
                                img.Open(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_recreational.png", ImageType.USE_FILE_EXTENSION, false, null);
                                cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                                cat.DrawingOptions.Picture = img;
                            }
                            else
                            {
                                cat.DrawingOptions.PointShape = tkPointShapeType.ptShapeFlag;
                                cat.DrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Cyan));
                                cat.DrawingOptions.PointSidesCount = 5;
                                cat.DrawingOptions.PointSidesRatio = (float)0.3;
                                cat.DrawingOptions.PointSize = (float)15;
                                cat.DrawingOptions.PointRotation = (double)0;
                                cat.DrawingOptions.LineVisible = true;
                                cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
                            }
                        }
                        else if (cat.Name == "RELIGIOUS ENTITIES")
                        {
                            boolreligious = true;
                            //img.Picture = IconConverter.GetIPictureDispFromImage(Properties.Resources.religious);
                            //img.Picture = myUtil.hBitmapToPicture(Properties.Resources.religious.GetHbitmap().ToInt32());
                            if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_religious.png"))
                            {
                                img.Open(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_religious.png", ImageType.USE_FILE_EXTENSION, false, null);
                                cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                                cat.DrawingOptions.Picture = img;
                            }
                            else
                            {
                                cat.DrawingOptions.PointShape = tkPointShapeType.ptShapeFlag;
                                cat.DrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.White));
                                cat.DrawingOptions.PointSidesCount = 5;
                                cat.DrawingOptions.PointSidesRatio = (float)0.3;
                                cat.DrawingOptions.PointSize = (float)15;
                                cat.DrawingOptions.PointRotation = (double)0;
                                cat.DrawingOptions.LineVisible = true;
                                cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
                            }
                        }
                        else if (cat.Name == "SUBDIVISION")
                        {
                            boolsubdivision = true;
                            //img.Picture = IconConverter.GetIPictureDispFromImage(Properties.Resources.subdivision);
                            //img.Picture = myUtil.hBitmapToPicture(Properties.Resources.subdivision.GetHbitmap().ToInt32());
                            if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_subdivision.png"))
                            {
                                img.Open(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_subdivision.png", ImageType.USE_FILE_EXTENSION, false, null);
                                cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                                cat.DrawingOptions.Picture = img;
                            }
                            else
                            {
                                cat.DrawingOptions.PointShape = tkPointShapeType.ptShapeFlag;
                                cat.DrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.YellowGreen));
                                cat.DrawingOptions.PointSidesCount = 5;
                                cat.DrawingOptions.PointSidesRatio = (float)0.3;
                                cat.DrawingOptions.PointSize = (float)15;
                                cat.DrawingOptions.PointRotation = (double)0;
                                cat.DrawingOptions.LineVisible = true;
                                cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
                            }
                        }
                        else if (cat.Name == "TRANSPORTATION ENTITIES")
                        {
                            booltransportation = true;
                            //img.Picture = IconConverter.GetIPictureDispFromImage(Properties.Resources.transportation);
                            //img.Picture = myUtil.hBitmapToPicture(Properties.Resources.transportation.GetHbitmap().ToInt32());
                            if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_transportation.png"))
                            {
                                img.Open(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_transportation.png", ImageType.USE_FILE_EXTENSION, false, null);
                                cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                                cat.DrawingOptions.Picture = img;
                            }
                            else
                            {
                                cat.DrawingOptions.PointShape = tkPointShapeType.ptShapeFlag;
                                cat.DrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.SpringGreen));
                                cat.DrawingOptions.PointSidesCount = 5;
                                cat.DrawingOptions.PointSidesRatio = (float)0.3;
                                cat.DrawingOptions.PointSize = (float)15;
                                cat.DrawingOptions.PointRotation = (double)0;
                                cat.DrawingOptions.LineVisible = true;
                                cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
                            }
                        }
                        else if (cat.Name == "TELECOMMUNICATION")
                        {
                            booltelecommunication = true;
                            //img.Picture = IconConverter.GetIPictureDispFromImage(Properties.Resources.telecommunication);
                            //img.Picture = myUtil.hBitmapToPicture(Properties.Resources.telecommunication.GetHbitmap().ToInt32());
                            if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_telecommunication.png"))
                            {
                                img.Open(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_telecommunication.png", ImageType.USE_FILE_EXTENSION, false, null);
                                cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                                cat.DrawingOptions.Picture = img;
                            }
                            else
                            {
                                cat.DrawingOptions.PointShape = tkPointShapeType.ptShapeFlag;
                                cat.DrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.RoyalBlue));
                                cat.DrawingOptions.PointSidesCount = 5;
                                cat.DrawingOptions.PointSidesRatio = (float)0.3;
                                cat.DrawingOptions.PointSize = (float)15;
                                cat.DrawingOptions.PointRotation = (double)0;
                                cat.DrawingOptions.LineVisible = true;
                                cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
                            }
                        }

                        //cat.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                        //cat.DrawingOptions.Picture = img;
                        cat.DrawingOptions.PictureScaleX = 0.8;
                        cat.DrawingOptions.PictureScaleY = 0.8;
                    }

                    //img = new MapWinGIS.Image();
                    //img.TransparencyColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.LightGray));
                    //img.TransparencyColor2 = Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray));
                    //img.UseTransparencyColor = true;

                    //fill the category gap or missing category from the auto generated one
                    ShapefileCategory cat1 = new ShapefileCategory();

                    if (boolcommercial == false)
                    {
                        cat1 = new ShapefileCategory();
                        cat1 = sf.Categories.Add("COMMERCIAL ENTITIES");
                        cat1.Expression = "[CATEGORY] = \"COMMERCIAL ENTITIES\"";
                        img = new MapWinGIS.Image();
                        img.TransparencyColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.LightGray));
                        img.TransparencyColor2 = Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray));
                        img.UseTransparencyColor = true;
                        //img.Picture = IconConverter.GetIPictureDispFromImage(Properties.Resources.commercial);
                        //img.Picture = myUtil.hBitmapToPicture(Properties.Resources.commercial.GetHbitmap().ToInt32());
                        //cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                        //cat1.DrawingOptions.Picture = img;
                        if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_commercial.png"))
                        {
                            img.Open(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_commercial.png", ImageType.USE_FILE_EXTENSION, false, null);
                            cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                            cat1.DrawingOptions.Picture = img;
                        }
                        else
                        {
                            cat1.DrawingOptions.PointShape = tkPointShapeType.ptShapeFlag;
                            cat1.DrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Red));
                            cat1.DrawingOptions.PointSidesCount = 5;
                            cat1.DrawingOptions.PointSidesRatio = (float)0.3;
                            cat1.DrawingOptions.PointSize = (float)15;
                            cat1.DrawingOptions.PointRotation = (double)0;
                            cat1.DrawingOptions.LineVisible = true;
                            cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
                        }
                        cat1.DrawingOptions.PictureScaleX = 0.8;
                        cat1.DrawingOptions.PictureScaleY = 0.8;
                    }

                    if (booleducational == false)
                    {
                        cat1 = new ShapefileCategory();
                        cat1 = sf.Categories.Add("EDUCATIONAL ENTITIES");
                        cat1.Expression = "[CATEGORY] = \"EDUCATIONAL ENTITIES\"";
                        img = new MapWinGIS.Image();
                        img.TransparencyColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.LightGray));
                        img.TransparencyColor2 = Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray));
                        img.UseTransparencyColor = true;
                        //img.Picture = IconConverter.GetIPictureDispFromImage(Properties.Resources.educational);
                        //img.Picture = myUtil.hBitmapToPicture(Properties.Resources.educational.GetHbitmap().ToInt32());
                        //cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                        //cat1.DrawingOptions.Picture = img;
                        if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_educational.png"))
                        {
                            img.Open(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_educational.png", ImageType.USE_FILE_EXTENSION, false, null);
                            cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                            cat1.DrawingOptions.Picture = img;
                        }
                        else
                        {
                            cat1.DrawingOptions.PointShape = tkPointShapeType.ptShapeFlag;
                            cat1.DrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Blue));
                            cat1.DrawingOptions.PointSidesCount = 5;
                            cat1.DrawingOptions.PointSidesRatio = (float)0.3;
                            cat1.DrawingOptions.PointSize = (float)15;
                            cat1.DrawingOptions.PointRotation = (double)0;
                            cat1.DrawingOptions.LineVisible = true;
                            cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
                        }
                        cat1.DrawingOptions.PictureScaleX = 0.8;
                        cat1.DrawingOptions.PictureScaleY = 0.8;
                    }

                    if (boolfinancial == false)
                    {
                        cat1 = new ShapefileCategory();
                        cat1 = sf.Categories.Add("FINANCIAL ENTITIES");
                        cat1.Expression = "[CATEGORY] = \"FINANCIAL ENTITIES\"";
                        img = new MapWinGIS.Image();
                        img.TransparencyColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.LightGray));
                        img.TransparencyColor2 = Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray));
                        img.UseTransparencyColor = true;
                        //img.Picture = IconConverter.GetIPictureDispFromImage(Properties.Resources.financial1);
                        //img.Picture = myUtil.hBitmapToPicture(Properties.Resources.financial1.GetHbitmap().ToInt32());
                        //cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                        //cat1.DrawingOptions.Picture = img;
                        if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_financial.png"))
                        {
                            img.Open(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_financial.png", ImageType.USE_FILE_EXTENSION, false, null);
                            cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                            cat1.DrawingOptions.Picture = img;
                        }
                        else
                        {
                            cat1.DrawingOptions.PointShape = tkPointShapeType.ptShapeFlag;
                            cat1.DrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Brown));
                            cat1.DrawingOptions.PointSidesCount = 5;
                            cat1.DrawingOptions.PointSidesRatio = (float)0.3;
                            cat1.DrawingOptions.PointSize = (float)15;
                            cat1.DrawingOptions.PointRotation = (double)0;
                            cat1.DrawingOptions.LineVisible = true;
                            cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
                        }
                        cat1.DrawingOptions.PictureScaleX = 0.8;
                        cat1.DrawingOptions.PictureScaleY = 0.8;
                    }

                    if (boolfire == false)
                    {
                        cat1 = new ShapefileCategory();
                        cat1 = sf.Categories.Add("FIRE STATION");
                        cat1.Expression = "[CATEGORY] = \"FIRE STATION\"";
                        img = new MapWinGIS.Image();
                        img.TransparencyColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.LightGray));
                        img.TransparencyColor2 = Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray));
                        img.UseTransparencyColor = true;
                        //img.Picture = IconConverter.GetIPictureDispFromImage(Properties.Resources.firestation);
                        //img.Picture = myUtil.hBitmapToPicture(Properties.Resources.firestation.GetHbitmap().ToInt32());
                        //cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                        //cat1.DrawingOptions.Picture = img;
                        if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_firestation.png"))
                        {
                            img.Open(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_firestation.png", ImageType.USE_FILE_EXTENSION, false, null);
                            cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                            cat1.DrawingOptions.Picture = img;
                        }
                        else
                        {
                            cat1.DrawingOptions.PointShape = tkPointShapeType.ptShapeFlag;
                            cat1.DrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Salmon));
                            cat1.DrawingOptions.PointSidesCount = 5;
                            cat1.DrawingOptions.PointSidesRatio = (float)0.3;
                            cat1.DrawingOptions.PointSize = (float)15;
                            cat1.DrawingOptions.PointRotation = (double)0;
                            cat1.DrawingOptions.LineVisible = true;
                            cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
                        }
                        cat1.DrawingOptions.PictureScaleX = 0.8;
                        cat1.DrawingOptions.PictureScaleY = 0.8;
                    }

                    if (boolgas == false)
                    {
                        cat1 = new ShapefileCategory();
                        cat1 = sf.Categories.Add("GAS STATION");
                        cat1.Expression = "[CATEGORY] = \"GAS STATION\"";
                        img = new MapWinGIS.Image();
                        img.TransparencyColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.LightGray));
                        img.TransparencyColor2 = Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray));
                        img.UseTransparencyColor = true;
                        //img.Picture = IconConverter.GetIPictureDispFromImage(Properties.Resources.gasstation);
                        //img.Picture = myUtil.hBitmapToPicture(Properties.Resources.gasstation.GetHbitmap().ToInt32());
                        //cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                        //cat1.DrawingOptions.Picture = img;
                        if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_gasstation.png"))
                        {
                            img.Open(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_gasstation.png", ImageType.USE_FILE_EXTENSION, false, null);
                            cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                            cat1.DrawingOptions.Picture = img;
                        }
                        else
                        {
                            cat1.DrawingOptions.PointShape = tkPointShapeType.ptShapeFlag;
                            cat1.DrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Silver));
                            cat1.DrawingOptions.PointSidesCount = 5;
                            cat1.DrawingOptions.PointSidesRatio = (float)0.3;
                            cat1.DrawingOptions.PointSize = (float)15;
                            cat1.DrawingOptions.PointRotation = (double)0;
                            cat1.DrawingOptions.LineVisible = true;
                            cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
                        }
                        cat1.DrawingOptions.PictureScaleX = 0.8;
                        cat1.DrawingOptions.PictureScaleY = 0.8;
                    }

                    if (boolgovernment == false)
                    {
                        cat1 = new ShapefileCategory();
                        cat1 = sf.Categories.Add("GOVERNMENT ENTITIES");
                        cat1.Expression = "[CATEGORY] = \"GOVERNMENT ENTITIES\"";
                        img = new MapWinGIS.Image();
                        img.TransparencyColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.LightGray));
                        img.TransparencyColor2 = Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray));
                        img.UseTransparencyColor = true;
                        //img.Picture = IconConverter.GetIPictureDispFromImage(Properties.Resources.government);
                        //img.Picture = myUtil.hBitmapToPicture(Properties.Resources.government.GetHbitmap().ToInt32());
                        //cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                        //cat1.DrawingOptions.Picture = img;
                        if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_government.png"))
                        {
                            img.Open(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_government.png", ImageType.USE_FILE_EXTENSION, false, null);
                            cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                            cat1.DrawingOptions.Picture = img;
                        }
                        else
                        {
                            cat1.DrawingOptions.PointShape = tkPointShapeType.ptShapeFlag;
                            cat1.DrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Tan));
                            cat1.DrawingOptions.PointSidesCount = 5;
                            cat1.DrawingOptions.PointSidesRatio = (float)0.3;
                            cat1.DrawingOptions.PointSize = (float)15;
                            cat1.DrawingOptions.PointRotation = (double)0;
                            cat1.DrawingOptions.LineVisible = true;
                            cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
                        }
                        cat1.DrawingOptions.PictureScaleX = 0.8;
                        cat1.DrawingOptions.PictureScaleY = 0.8;
                    }

                    if (boolindustrial == false)
                    {
                        cat1 = new ShapefileCategory();
                        cat1 = sf.Categories.Add("INDUSTRIAL ENTITIES");
                        cat1.Expression = "[CATEGORY] = \"INDUSTRIAL ENTITIES\"";
                        img = new MapWinGIS.Image();
                        img.TransparencyColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.LightGray));
                        img.TransparencyColor2 = Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray));
                        img.UseTransparencyColor = true;
                        //img.Picture = IconConverter.GetIPictureDispFromImage(Properties.Resources.industrial);
                        //img.Picture = myUtil.hBitmapToPicture(Properties.Resources.industrial.GetHbitmap().ToInt32());
                        //cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                        //cat1.DrawingOptions.Picture = img;
                        if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_industrial.png"))
                        {
                            img.Open(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_industrial.png", ImageType.USE_FILE_EXTENSION, false, null);
                            cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                            cat1.DrawingOptions.Picture = img;
                        }
                        else
                        {
                            cat1.DrawingOptions.PointShape = tkPointShapeType.ptShapeFlag;
                            cat1.DrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Purple));
                            cat1.DrawingOptions.PointSidesCount = 5;
                            cat1.DrawingOptions.PointSidesRatio = (float)0.3;
                            cat1.DrawingOptions.PointSize = (float)15;
                            cat1.DrawingOptions.PointRotation = (double)0;
                            cat1.DrawingOptions.LineVisible = true;
                            cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
                        }
                        cat1.DrawingOptions.PictureScaleX = 0.8;
                        cat1.DrawingOptions.PictureScaleY = 0.8;
                    }

                    if (boolmedical == false)
                    {
                        cat1 = new ShapefileCategory();
                        cat1 = sf.Categories.Add("MEDICAL ENTITIES");
                        cat1.Expression = "[CATEGORY] = \"MEDICAL ENTITIES\"";
                        img = new MapWinGIS.Image();
                        img.TransparencyColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.LightGray));
                        img.TransparencyColor2 = Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray));
                        img.UseTransparencyColor = true;
                        //img.Picture = IconConverter.GetIPictureDispFromImage(Properties.Resources.medical);
                        //img.Picture = myUtil.hBitmapToPicture(Properties.Resources.medical.GetHbitmap().ToInt32());
                        //cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                        //cat1.DrawingOptions.Picture = img;
                        if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_medical.png"))
                        {
                            img.Open(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_medical.png", ImageType.USE_FILE_EXTENSION, false, null);
                            cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                            cat1.DrawingOptions.Picture = img;
                        }
                        else
                        {
                            cat1.DrawingOptions.PointShape = tkPointShapeType.ptShapeFlag;
                            cat1.DrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Yellow));
                            cat1.DrawingOptions.PointSidesCount = 5;
                            cat1.DrawingOptions.PointSidesRatio = (float)0.3;
                            cat1.DrawingOptions.PointSize = (float)15;
                            cat1.DrawingOptions.PointRotation = (double)0;
                            cat1.DrawingOptions.LineVisible = true;
                            cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
                        }
                        cat1.DrawingOptions.PictureScaleX = 0.8;
                        cat1.DrawingOptions.PictureScaleY = 0.8;
                    }
                    if (boolothers == false)
                    {
                        cat1 = new ShapefileCategory();
                        cat1 = sf.Categories.Add("OTHERS");
                        cat1.Expression = "[CATEGORY] = \"OTHERS\"";
                        img = new MapWinGIS.Image();
                        img.TransparencyColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.LightGray));
                        img.TransparencyColor2 = Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray));
                        img.UseTransparencyColor = true;
                        //img.Picture = IconConverter.GetIPictureDispFromImage(Properties.Resources.others);
                        //img.Picture = myUtil.hBitmapToPicture(Properties.Resources.others.GetHbitmap().ToInt32());
                        //cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                        //cat1.DrawingOptions.Picture = img;
                        if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_others.png"))
                        {
                            img.Open(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_others.png", ImageType.USE_FILE_EXTENSION, false, null);
                            cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                            cat1.DrawingOptions.Picture = img;
                        }
                        else
                        {
                            cat1.DrawingOptions.PointShape = tkPointShapeType.ptShapeFlag;
                            cat1.DrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Orange));
                            cat1.DrawingOptions.PointSidesCount = 5;
                            cat1.DrawingOptions.PointSidesRatio = (float)0.3;
                            cat1.DrawingOptions.PointSize = (float)15;
                            cat1.DrawingOptions.PointRotation = (double)0;
                            cat1.DrawingOptions.LineVisible = true;
                            cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
                        }
                        cat1.DrawingOptions.PictureScaleX = 0.8;
                        cat1.DrawingOptions.PictureScaleY = 0.8;
                    }
                    if (boolpolice == false)
                    {
                        cat1 = new ShapefileCategory();
                        cat1 = sf.Categories.Add("POLICE STATION");
                        cat1.Expression = "[CATEGORY] = \"POLICE STATION\"";
                        img = new MapWinGIS.Image();
                        img.TransparencyColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.LightGray));
                        img.TransparencyColor2 = Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray));
                        img.UseTransparencyColor = true;
                       // img.Picture = IconConverter.GetIPictureDispFromImage(Properties.Resources.police);
                        //img.Picture = myUtil.hBitmapToPicture(Properties.Resources.police.GetHbitmap().ToInt32());
                        //cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                        //cat1.DrawingOptions.Picture = img;
                        if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_police.png"))
                        {
                            img.Open(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_police.png", ImageType.USE_FILE_EXTENSION, false, null);
                            cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                            cat1.DrawingOptions.Picture = img;
                        }
                        else
                        {
                            cat1.DrawingOptions.PointShape = tkPointShapeType.ptShapeFlag;
                            cat1.DrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Brown));
                            cat1.DrawingOptions.PointSidesCount = 5;
                            cat1.DrawingOptions.PointSidesRatio = (float)0.3;
                            cat1.DrawingOptions.PointSize = (float)15;
                            cat1.DrawingOptions.PointRotation = (double)0;
                            cat1.DrawingOptions.LineVisible = true;
                            cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
                        }
                        cat1.DrawingOptions.PictureScaleX = 0.8;
                        cat1.DrawingOptions.PictureScaleY = 0.8;
                    }
                    if (boolrecreational == false)
                    {
                        cat1 = new ShapefileCategory();
                        cat1 = sf.Categories.Add("RECREATIONAL ENTITIES");
                        cat1.Expression = "[CATEGORY] = \"RECREATIONAL ENTITIES\"";
                        img = new MapWinGIS.Image();
                        img.TransparencyColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.LightGray));
                        img.TransparencyColor2 = Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray));
                        img.UseTransparencyColor = true;
                       // img.Picture = IconConverter.GetIPictureDispFromImage(Properties.Resources.recreational);
                        //img.Picture = myUtil.hBitmapToPicture(Properties.Resources.recreational.GetHbitmap().ToInt32());
                        //cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                        //cat1.DrawingOptions.Picture = img;
                        if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_recreational.png"))
                        {
                            img.Open(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_recreational.png", ImageType.USE_FILE_EXTENSION, false, null);
                            cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                            cat1.DrawingOptions.Picture = img;
                        }
                        else
                        {
                            cat1.DrawingOptions.PointShape = tkPointShapeType.ptShapeFlag;
                            cat1.DrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Cyan));
                            cat1.DrawingOptions.PointSidesCount = 5;
                            cat1.DrawingOptions.PointSidesRatio = (float)0.3;
                            cat1.DrawingOptions.PointSize = (float)15;
                            cat1.DrawingOptions.PointRotation = (double)0;
                            cat1.DrawingOptions.LineVisible = true;
                            cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
                        }
                        cat1.DrawingOptions.PictureScaleX = 0.8;
                        cat1.DrawingOptions.PictureScaleY = 0.8;
                    }
                    if (boolreligious == false)
                    {
                        cat1 = new ShapefileCategory();
                        cat1 = sf.Categories.Add("RELIGIOUS ENTITIES");
                        cat1.Expression = "[CATEGORY] = \"RELIGIOUS ENTITIES\"";
                        img = new MapWinGIS.Image();
                        img.TransparencyColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.LightGray));
                        img.TransparencyColor2 = Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray));
                        img.UseTransparencyColor = true;
                        //img.Picture = IconConverter.GetIPictureDispFromImage(Properties.Resources.religious);
                        //img.Picture = myUtil.hBitmapToPicture(Properties.Resources.religious.GetHbitmap().ToInt32());
                        //cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                        //cat1.DrawingOptions.Picture = img;
                        if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_religious.png"))
                        {
                            img.Open(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_religious.png", ImageType.USE_FILE_EXTENSION, false, null);
                            cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                            cat1.DrawingOptions.Picture = img;
                        }
                        else
                        {
                            cat1.DrawingOptions.PointShape = tkPointShapeType.ptShapeFlag;
                            cat1.DrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.White));
                            cat1.DrawingOptions.PointSidesCount = 5;
                            cat1.DrawingOptions.PointSidesRatio = (float)0.3;
                            cat1.DrawingOptions.PointSize = (float)15;
                            cat1.DrawingOptions.PointRotation = (double)0;
                            cat1.DrawingOptions.LineVisible = true;
                            cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
                        }
                        cat1.DrawingOptions.PictureScaleX = 0.8;
                        cat1.DrawingOptions.PictureScaleY = 0.8;
                    }
                    if (boolsubdivision == false)
                    {
                        cat1 = new ShapefileCategory();
                        cat1 = sf.Categories.Add("SUBDIVISION");
                        cat1.Expression = "[CATEGORY] = \"SUBDIVISION\"";
                        img = new MapWinGIS.Image();
                        img.TransparencyColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.LightGray));
                        img.TransparencyColor2 = Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray));
                        img.UseTransparencyColor = true;
                        //img.Picture = IconConverter.GetIPictureDispFromImage(Properties.Resources.subdivision);
                       // img.Picture = myUtil.hBitmapToPicture(Properties.Resources.subdivision.GetHbitmap().ToInt32());
                        //cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                        //cat1.DrawingOptions.Picture = img;
                        if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_subdivision.png"))
                        {
                            img.Open(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_subdivision.png", ImageType.USE_FILE_EXTENSION, false, null);
                            cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                            cat1.DrawingOptions.Picture = img;
                        }
                        else
                        {
                            cat1.DrawingOptions.PointShape = tkPointShapeType.ptShapeFlag;
                            cat1.DrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.YellowGreen));
                            cat1.DrawingOptions.PointSidesCount = 5;
                            cat1.DrawingOptions.PointSidesRatio = (float)0.3;
                            cat1.DrawingOptions.PointSize = (float)15;
                            cat1.DrawingOptions.PointRotation = (double)0;
                            cat1.DrawingOptions.LineVisible = true;
                            cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
                        }
                        cat1.DrawingOptions.PictureScaleX = 0.8;
                        cat1.DrawingOptions.PictureScaleY = 0.8;
                    }
                    if (booltransportation == false)
                    {
                        cat1 = new ShapefileCategory();
                        cat1 = sf.Categories.Add("TRANSPORTATION ENTITIES");
                        cat1.Expression = "[CATEGORY] = \"TRANSPORTATION ENTITIES\"";
                        img = new MapWinGIS.Image();
                        img.TransparencyColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.LightGray));
                        img.TransparencyColor2 = Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray));
                        img.UseTransparencyColor = true;
                        //img.Picture = IconConverter.GetIPictureDispFromImage(Properties.Resources.transportation);
                        //img.Picture = myUtil.hBitmapToPicture(Properties.Resources.transportation.GetHbitmap().ToInt32());
                        //cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                        //cat1.DrawingOptions.Picture = img;
                        if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_transportation.png"))
                        {
                            img.Open(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_transportation.png", ImageType.USE_FILE_EXTENSION, false, null);
                            cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                            cat1.DrawingOptions.Picture = img;
                        }
                        else
                        {
                            cat1.DrawingOptions.PointShape = tkPointShapeType.ptShapeFlag;
                            cat1.DrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.SpringGreen));
                            cat1.DrawingOptions.PointSidesCount = 5;
                            cat1.DrawingOptions.PointSidesRatio = (float)0.3;
                            cat1.DrawingOptions.PointSize = (float)15;
                            cat1.DrawingOptions.PointRotation = (double)0;
                            cat1.DrawingOptions.LineVisible = true;
                            cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
                        }
                        cat1.DrawingOptions.PictureScaleX = 0.8;
                        cat1.DrawingOptions.PictureScaleY = 0.8;
                    }
                    if (booltelecommunication == false)
                    {
                        cat1 = new ShapefileCategory();
                        cat1 = sf.Categories.Add("TELECOMMUNICATION");
                        cat1.Expression = "[CATEGORY] = \"TELECOMMUNICATION\"";
                        img = new MapWinGIS.Image();
                        img.TransparencyColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.LightGray));
                        img.TransparencyColor2 = Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray));
                        img.UseTransparencyColor = true;
                        //img.Picture = IconConverter.GetIPictureDispFromImage(Properties.Resources.telecommunication);
                        //img.Picture = myUtil.hBitmapToPicture(Properties.Resources.telecommunication.GetHbitmap().ToInt32());
                        //cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                       // cat1.DrawingOptions.Picture = img;
                        if (File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_telecommunication.png"))
                        {
                            img.Open(Path.GetDirectoryName(Application.ExecutablePath) + @"\images\landmark\landmark_telecommunication.png", ImageType.USE_FILE_EXTENSION, false, null);
                            cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                            cat1.DrawingOptions.Picture = img;
                        }
                        else
                        {
                            cat1.DrawingOptions.PointShape = tkPointShapeType.ptShapeFlag;
                            cat1.DrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.RoyalBlue));
                            cat1.DrawingOptions.PointSidesCount = 5;
                            cat1.DrawingOptions.PointSidesRatio = (float)0.3;
                            cat1.DrawingOptions.PointSize = (float)15;
                            cat1.DrawingOptions.PointRotation = (double)0;
                            cat1.DrawingOptions.LineVisible = true;
                            cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolStandard;
                        }
                        cat1.DrawingOptions.PictureScaleX = 0.8;
                        cat1.DrawingOptions.PictureScaleY = 0.8;
                    }

                    //cat1.DrawingOptions.PointType = tkPointSymbolType.ptSymbolPicture;
                    //cat1.DrawingOptions.Picture = img;
                    //cat1.DrawingOptions.PictureScaleX = 0.8;
                    //cat1.DrawingOptions.PictureScaleY = 0.8;

                    sf.CollisionMode = tkCollisionMode.AllowCollisions;

            
                    //sorting of categories as prefererred order
                    for (int i = 0; i < sf.Categories.Count; i++)
                    {
                        ShapefileCategory cat = sf.Categories.get_Item(i);
                        if (cat.Name == "COMMERCIAL ENTITIES")
                        {
                            if (i > 1)
                            {
                                do
                                {
                                    sf.Categories.MoveUp(i);
                                    i -= 1;
                                } while (i != 1);
                            }
                            else
                                sf.Categories.MoveDown(i);
                        }
                    }
                    for (int i = 0; i < sf.Categories.Count; i++)
                    {
                        ShapefileCategory cat = sf.Categories.get_Item(i);
                        if (cat.Name == "EDUCATIONAL ENTITIES")
                        {
                            if (i > 2)
                            {
                                do
                                {
                                    sf.Categories.MoveUp(i);
                                    i -= 1;
                                } while (i != 2);
                            }
                            else if (i < 2)
                            {
                                do
                                {
                                    sf.Categories.MoveDown(i);
                                    i += 1;
                                } while (i != 2);
                            }
                        }
                    }
                    for (int i = 0; i < sf.Categories.Count; i++)
                    {
                        ShapefileCategory cat = sf.Categories.get_Item(i);
                        if (cat.Name == "FINANCIAL ENTITIES")
                        {
                            if (i > 3)
                            {
                                do
                                {
                                    sf.Categories.MoveUp(i);
                                    i -= 1;
                                } while (i != 3);
                            }
                            else if (i < 3)
                            {
                                do
                                {
                                    sf.Categories.MoveDown(i);
                                    i += 1;
                                } while (i != 3);
                            }
                        }
                    }
                    for (int i = 0; i < sf.Categories.Count; i++)
                    {
                        ShapefileCategory cat = sf.Categories.get_Item(i);
                        if (cat.Name == "GAS STATION")
                        {
                            if (i > 4)
                            {
                                do
                                {
                                    sf.Categories.MoveUp(i);
                                    i -= 1;
                                } while (i != 4);
                            }
                            else if (i < 4)
                            {
                                do
                                {
                                    sf.Categories.MoveDown(i);
                                    i += 1;
                                } while (i != 4);
                            }
                        }
                    }
                    for (int i = 0; i < sf.Categories.Count; i++)
                    {
                        ShapefileCategory cat = sf.Categories.get_Item(i);
                        if (cat.Name == "GOVERNMENT ENTITIES")
                        {
                            if (i > 5)
                            {
                                do
                                {
                                    sf.Categories.MoveUp(i);
                                    i -= 1;
                                } while (i != 5);
                            }
                            else if (i < 5)
                            {
                                do
                                {
                                    sf.Categories.MoveDown(i);
                                    i += 1;
                                } while (i != 5);
                            }
                        }
                    }
                    for (int i = 0; i < sf.Categories.Count; i++)
                    {
                        ShapefileCategory cat = sf.Categories.get_Item(i);
                        if (cat.Name == "INDUSTRIAL ENTITIES")
                        {
                            if (i > 6)
                            {
                                do
                                {
                                    sf.Categories.MoveUp(i);
                                    i -= 1;
                                } while (i != 6);
                            }
                            else if (i < 6)
                            {
                                do
                                {
                                    sf.Categories.MoveDown(i);
                                    i += 1;
                                } while (i != 6);
                            }
                        }
                    }
                    for (int i = 0; i < sf.Categories.Count; i++)
                    {
                        ShapefileCategory cat = sf.Categories.get_Item(i);
                        if (cat.Name == "MEDICAL ENTITIES")
                        {
                            if (i > 7)
                            {
                                do
                                {
                                    sf.Categories.MoveUp(i);
                                    i -= 1;
                                } while (i != 7);
                            }
                            else if (i < 7)
                            {
                                do
                                {
                                    sf.Categories.MoveDown(i);
                                    i += 1;
                                } while (i != 7);
                            }
                        }
                    }
                    for (int i = 0; i < sf.Categories.Count; i++)
                    {
                        ShapefileCategory cat = sf.Categories.get_Item(i);
                        if (cat.Name == "OTHERS")
                        {
                            if (i > 8)
                            {
                                do
                                {
                                    sf.Categories.MoveUp(i);
                                    i -= 1;
                                } while (i != 8);
                            }
                            else if (i < 8)
                            {
                                do
                                {
                                    sf.Categories.MoveDown(i);
                                    i += 1;
                                } while (i != 8);
                            }
                        }
                    }
                    for (int i = 0; i < sf.Categories.Count; i++)
                    {
                        ShapefileCategory cat = sf.Categories.get_Item(i);
                        if (cat.Name == "POLICE STATION")
                        {
                            if (i > 9)
                            {
                                do
                                {
                                    sf.Categories.MoveUp(i);
                                    i -= 1;
                                } while (i != 9);
                            }
                            else if (i < 9)
                            {
                                do
                                {
                                    sf.Categories.MoveDown(i);
                                    i += 1;
                                } while (i != 9);
                            }
                        }
                    }
                    for (int i = 0; i < sf.Categories.Count; i++)
                    {
                        ShapefileCategory cat = sf.Categories.get_Item(i);
                        if (cat.Name == "RECREATIONAL ENTITIES")
                        {
                            if (i > 10)
                            {
                                do
                                {
                                    sf.Categories.MoveUp(i);
                                    i -= 1;
                                } while (i != 10);
                            }
                            else if (i < 10)
                            {
                                do
                                {
                                    sf.Categories.MoveDown(i);
                                    i += 1;
                                } while (i != 10);
                            }
                        }
                    }
                    for (int i = 0; i < sf.Categories.Count; i++)
                    {
                        ShapefileCategory cat = sf.Categories.get_Item(i);
                        if (cat.Name == "RELIGIOUS ENTITIES")
                        {
                            if (i > 11)
                            {
                                do
                                {
                                    sf.Categories.MoveUp(i);
                                    i -= 1;
                                } while (i != 11);
                            }
                            else if (i < 11)
                            {
                                do
                                {
                                    sf.Categories.MoveDown(i);
                                    i += 1;
                                } while (i != 11);
                            }
                        }
                    }
                    for (int i = 0; i < sf.Categories.Count; i++)
                    {
                        ShapefileCategory cat = sf.Categories.get_Item(i);
                        if (cat.Name == "SUBDIVISION")
                        {
                            if (i > 12)
                            {
                                do
                                {
                                    sf.Categories.MoveUp(i);
                                    i -= 1;
                                } while (i != 12);
                            }
                            else if (i < 12)
                            {
                                do
                                {
                                    sf.Categories.MoveDown(i);
                                    i += 1;
                                } while (i != 12);
                            }
                        }
                    }
                    for (int i = 0; i < sf.Categories.Count; i++)
                    {
                        ShapefileCategory cat = sf.Categories.get_Item(i);
                        if (cat.Name == "TRANSPORTATION ENTITIES")
                        {
                            if (i > 13)
                            {
                                do
                                {
                                    sf.Categories.MoveUp(i);
                                    i -= 1;
                                } while (i != 13);
                            }
                            else if (i < 13)
                            {
                                do
                                {
                                    sf.Categories.MoveDown(i);
                                    i += 1;
                                } while (i != 13);
                            }
                        }
                    }
                    for (int i = 0; i < sf.Categories.Count; i++)
                    {
                        ShapefileCategory cat = sf.Categories.get_Item(i);
                        if (cat.Name == "TELECOMMUNICATION")
                        {
                            if (i > 14)
                            {
                                do
                                {
                                    sf.Categories.MoveUp(i);
                                    i -= 1;
                                } while (i != 14);
                            }
                            else if (i < 14)
                            {
                                do
                                {
                                    sf.Categories.MoveDown(i);
                                    i += 1;
                                } while (i != 14);
                            }
                        }
                    }

                    sf.Categories.ApplyExpressions();
                    sf.Categories.Caption = "CATEGORIES";

                    sf.DefaultDrawingOptions.FillVisible = false;
                    sf.DefaultDrawingOptions.Visible = true;
                    sf.DefaultDrawingOptions.LineVisible = false;
                    sf.DefaultDrawingOptions.PointRotation = (double)0;
                    sf.DefaultDrawingOptions.PointSidesRatio = (float)0.5;

                    ////sf.StopEditingTable(true, null);
                    //sf.StopEditingShapes(true, true, null);

                    //m_frmM.legend1.Layers.ItemByHandle(m_frmM.hndMap).Refresh();
                    m_frmM.axMap.Refresh();
                    m_frmM.legend1.Refresh();

                    sftable.Close();
                    //if (sf.EditingTable)
                    //    sf.StopEditingTable(false,null);
                    //if (sf.EditingShapes)
                    //    sf.StopEditingShapes(false, false, null);

                    Cursor.Current = Cursors.Default;
                    return sf;
                }
                return sf;
            }
            return sf;
        }

        public void VerifyShape4(MapWinGIS.Shape s, string dsnOLEDB)
        {
            //this arraylist will be the reference for the removal/undo of the subd of parcels
            ArrayList newshpindxcoll = new ArrayList();
            ArrayList mothershpindxcoll = new ArrayList();
            ArrayList mothershpincoll = new ArrayList();
            ArrayList mothershpindxcoll2 = new ArrayList();
            string strmotherpin = String.Empty;
            string strtestsect = String.Empty;
            string strtestpin = String.Empty;
            string sdbaselastpin = String.Empty;
            string sshapelastpin = String.Empty;
            string ssuggestednextpin = String.Empty;
            Dictionary<string, int> pins = new Dictionary<string, int>();

            try
            {
                //int intmotheridx = -1;
                if (s.numPoints < 2)
                {
                    resetLine("KML");
                    Logger.Message("You must add at least two points for a line.", "Not Enough Points", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, DialogResult.OK);
                    return;
                }

                Shapefile newsf = new MapWinGIS.Shapefile();
                if (System.IO.File.Exists("new_temp_kml_shp.shp"))
                    System.IO.File.Delete("new_temp_kml_shp.shp");
                if (System.IO.File.Exists("new_temp_kml_shp.dbf"))
                    System.IO.File.Delete("new_temp_kml_shp.dbf");
                if (System.IO.File.Exists("new_temp_kml_shp.shx"))
                    System.IO.File.Delete("new_temp_kml_shp.shx");
                if (System.IO.File.Exists("new_temp_kml_shp.prj"))
                    System.IO.File.Delete("new_temp_kml_shp.prj");

                bool results = newsf.CreateNew("new_temp_kml_shp.shp", MapWinGIS.ShpfileType.SHP_POLYLINE);
                if (!results)
                {
                    MessageBox.Show("Failed to create a new shapefile. \n\r" + newsf.get_ErrorMsg(newsf.LastErrorCode), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    int intfld = 0;
                    //create fieldnames
                    MapWinGIS.Field fld = new MapWinGIS.FieldClass();
                    fld.Name = "MWShapeID";
                    fld.Width = 10;
                    fld.Type = MapWinGIS.FieldType.INTEGER_FIELD;
                    newsf.EditInsertField(fld, ref intfld, null);
                }

                object ShapeIDS = new object();
                Shapefile sf = (Shapefile)frmM.axMap.get_GetObject(frmM.hndMapSel);
                Extents ext = frmM.m_newlineshape.Extents;

                object newShapeIDS = new object();
                int newshpidx = 1;
                if (results)
                {
                    results = newsf.StartEditingShapes(true, null);
                    newsf.EditInsertShape(s, ref newshpidx);
                }
                if (results)
                    results = newsf.StopEditingShapes(true, true, null);
                sf.SelectByShapefile(newsf, tkSpatialRelation.srCrosses, false, ref newShapeIDS, null);
                int[] newselshapes = (int[])newShapeIDS;

                int pinfldidx = -1;
                pinfldidx = sf.Table.get_FieldIndexByName("PIN");
                if (pinfldidx == -1)
                {
                    MessageBox.Show("Can't find the PIN field, \n\rplease check the shapefile table.");
                    return;
                }

                for (int i = 0; i < newselshapes.Length; i++)
                {
                    strmotherpin = sf.Table.get_CellValue(pinfldidx, Convert.ToInt32(newselshapes.GetValue(i))).ToString();
                    if (strmotherpin != "")
                        mothershpincoll.Add(strmotherpin);
                    mothershpindxcoll.Add(Convert.ToInt32(newselshapes.GetValue(i)));
                    pins.Add(strmotherpin, Convert.ToInt32(newselshapes.GetValue(i)));
                }

                foreach (string strpin in mothershpincoll)
                {
                    //if(mothershpincoll[0].ToString().Length == 18)
                    if (strpin.Length == 18)
                    {
                        strtestpin = strpin;
                        strtestsect = strpin.Substring(0, 14);
                        break;
                    }
                }

                //if (strtestsect != "")
                //{
                //    sdbaselastpin = GetDBaseLastPin(strtestpin, dsnOLEDB);
                //    sshapelastpin = GetShapeLastPIN(strtestpin, sf);

                //    if (Convert.ToInt32(sdbaselastpin.Substring(15, 3)) > Convert.ToInt32(sshapelastpin.Substring(15, 3)))
                //        ssuggestednextpin = sdbaselastpin.Substring(0, 15) + String.Format("{0:D3}", Convert.ToInt32(sdbaselastpin.Substring(15, 3)) + 1);
                //    else
                //        ssuggestednextpin = sshapelastpin.Substring(0, 15) + String.Format("{0:D3}", Convert.ToInt32(sshapelastpin.Substring(15, 3)) + 1);
                //}

                if (File.Exists(newsf.Filename))
                {
                    string newshpfilename = newsf.Filename.ToString();
                    MapWinGeoProc.DataManagement.DeleteShapefile(ref newshpfilename);
                }



                if (frmgooglekml == null)
                {
                    frmgooglekml = new frmGoogleEarthViewer();
                    frmgooglekml.Owner = frmM;
                }

                if (pins.Count > 0)
                    frmgooglekml.frmInit(frmM, this, sf, pins);
                else
                {
                    MessageBox.Show("Only one(1) parcel was selected \n\rwhile creating the line, \n\rplease repick you selection.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    frmM.axMap.ClearDrawing(frmM.hndMapSel);
                }
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message.ToString(), frmgooglekml.Text);

            }
        }

        public void VerifyShape5(MapWinGIS.Shape s, string dsnOLEDB)
        {
            //this arraylist will be the reference for the removal/undo of the subd of parcels
            ArrayList newshpindxcoll = new ArrayList();
            ArrayList mothershpindxcoll = new ArrayList();
            ArrayList mothershpincoll = new ArrayList();
            ArrayList mothershpindxcoll2 = new ArrayList();
            string strmotherpin = String.Empty;
            string strtestsect = String.Empty;
            string strtestpin = String.Empty;
            string sdbaselastpin = String.Empty;
            string sshapelastpin = String.Empty;
            string ssuggestednextpin = String.Empty;
            Dictionary<string, int> pins = new Dictionary<string, int>();

            try
            {
                if (s.numPoints < 2)
                {
                    resetLine("GRLINE");
                    Logger.Message("You must add at least two points for a line.", "Not Enough Points", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, DialogResult.OK);
                    return;
                }

                Shapefile newsf = new MapWinGIS.Shapefile();
                if (System.IO.File.Exists("new_temp_gr_shp.shp"))
                    System.IO.File.Delete("new_temp_gr_shp.shp");
                if (System.IO.File.Exists("new_temp_gr_shp.dbf"))
                    System.IO.File.Delete("new_temp_gr_shp.dbf");
                if (System.IO.File.Exists("new_temp_gr_shp.shx"))
                    System.IO.File.Delete("new_temp_gr_shp.shx");
                if (System.IO.File.Exists("new_temp_gr_shp.prj"))
                    System.IO.File.Delete("new_temp_gr_shp.prj");

                bool results = newsf.CreateNew("new_temp_gr_shp.shp", MapWinGIS.ShpfileType.SHP_POLYLINE);
                if (!results)
                {
                    MessageBox.Show("Failed to create a new shapefile. \n\r" + newsf.get_ErrorMsg(newsf.LastErrorCode), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    int intfld = 0;
                    //create fieldnames
                    MapWinGIS.Field fld = new MapWinGIS.FieldClass();
                    fld.Name = "MWShapeID";
                    fld.Width = 10;
                    fld.Type = MapWinGIS.FieldType.INTEGER_FIELD;
                    newsf.EditInsertField(fld, ref intfld, null);
                }

                object ShapeIDS = new object();
                Shapefile sf = (Shapefile)frmM.axMap.get_GetObject(frmM.hndMapSel);
                Extents ext = frmM.m_newlineshape.Extents;

                object newShapeIDS = new object();
                int newshpidx = 1;
                if (results)
                {
                    results = newsf.StartEditingShapes(true, null);
                    newsf.EditInsertShape(s, ref newshpidx);
                }
                if (results)
                    results = newsf.StopEditingShapes(true, true, null);
                sf.SelectByShapefile(newsf, tkSpatialRelation.srIntersects, false, ref newShapeIDS, null);
                int[] newselshapes = (int[])newShapeIDS;

                int pinfldidx = -1;
                pinfldidx = sf.Table.get_FieldIndexByName("PIN");
                if (pinfldidx == -1)
                {
                    MessageBox.Show("Can't find the PIN field, \n\rplease check the shapefile table.");
                    return;
                }

                for (int i = 0; i < newselshapes.Length; i++)
                {
                    strmotherpin = sf.Table.get_CellValue(pinfldidx, Convert.ToInt32(newselshapes.GetValue(i))).ToString();
                    if (strmotherpin != "")
                        mothershpincoll.Add(strmotherpin);
                    mothershpindxcoll.Add(Convert.ToInt32(newselshapes.GetValue(i)));
                    pins.Add(strmotherpin, Convert.ToInt32(newselshapes.GetValue(i)));
                }

                foreach (string strpin in mothershpincoll)
                {
                    if (strpin.Length == 18)
                    {
                        strtestpin = strpin;
                        strtestsect = strpin.Substring(0, 14);
                        break;
                    }
                }

                if (File.Exists(newsf.Filename))
                {
                    string newshpfilename = newsf.Filename.ToString();
                    MapWinGeoProc.DataManagement.DeleteShapefile(ref newshpfilename);
                }

                if (frmvaluation == null)
                {
                    frmvaluation = new frmValuation();
                    frmvaluation.Owner = frmM;
                }

                if (pins.Count > 0)
                {
                    frmvaluation.frmInitLineParcel(this, frmM, sf, pins, "SHAPEFILECALL");
                    frmvaluation.Show();
                }
                else
                {
                    MessageBox.Show("Only one(1) parcel was selected \n\rwhile creating the line, \n\rplease repick you selection.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    frmM.axMap.ClearDrawing(frmM.hndMapSel);
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message.ToString(), frmvaluation.Text);

            }
        }

        public void VerifyShape6(MapWinGIS.Shape s, string dsnOLEDB)
        {
            ArrayList newshpindxcoll = new ArrayList();//edited 10/17/2013
            ArrayList mothershpindxcoll = new ArrayList();//edited 10/17/2013
            if (frmsubd != null)
            {
                if (frmsubd.Visible == false)
                {
                    newshpindxcoll = new ArrayList();
                    mothershpindxcoll = new ArrayList();
                }
            }
            //ArrayList newshpindxcoll = new ArrayList();////remarked 9/17/2013
            //ArrayList mothershpindxcoll = new ArrayList();////remarked 9/17/2013
            string strmotherpin = String.Empty;
            int intmotheridx = -1;
            if (s.numPoints < 2)
            {
                resetLine("SUBDLAND");
                Logger.Message("You must add at least two points for a line.", "Not Enough Points", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, DialogResult.OK);
                return;
            }

            Shapefile newsf = new MapWinGIS.Shapefile();
            if (System.IO.File.Exists("new_temp_subd_shp.shp"))
                System.IO.File.Delete("new_temp_subd_shp.shp");
            if (System.IO.File.Exists("new_temp_subd_shp.dbf"))
                System.IO.File.Delete("new_temp_subd_shp.dbf");
            if (System.IO.File.Exists("new_temp_subd_shp.shx"))
                System.IO.File.Delete("new_temp_subd_shp.shx");
            if (System.IO.File.Exists("new_temp_subd_shp.prj"))
                System.IO.File.Delete("new_temp_subd_shp.prj");

            bool results = newsf.CreateNew("new_temp_subd_shp.shp", MapWinGIS.ShpfileType.SHP_POLYGON);
            object ShapeIDS = new object();
            Shapefile sf = (Shapefile)frmM.axMap.get_GetObject(frmM.hndMapSel);
            Extents ext = frmM.m_newlineshape.Extents;

            try
            {
                if (sf.SelectShapes(ext, (double)0, SelectMode.INTERSECTION, ref ShapeIDS))
                {
                    int[] shapes = (int[])ShapeIDS;
                    if (shapes.Length > 0)
                    {
                        string lastpin = String.Empty;
                        for (int i = 0; i < shapes.Length; i++)
                        {
                            MapWinGIS.Shape polyshp = sf.get_Shape(Convert.ToInt32(shapes.GetValue(i)));
                            MapWinGIS.Shape lineshp = frmM.m_newlineshape;
                            strmotherpin = sf.Table.get_CellValue(sf.Table.get_FieldIndexByName("PIN"), Convert.ToInt32(shapes.GetValue(i))).ToString();
                            intmotheridx = Convert.ToInt32(shapes.GetValue(i));
                            try
                            {
                                //if (MapWinGeoProc.SpatialOperations.ClipPolygonWithLine(ref polyshp, ref lineshp, out newsf))
                                //if(SectionPolygonWithLine(lineshp,polyshp,0,2,newsf))
                                //{
                                //}

                                for (int x = 0; x < polyshp.NumParts; x++)
                                {
                                    //if(lineshp.inpolyshp.get_Part(x);
                                    
                                }
                            
                                //MapWinGeoProc.SpatialOperations.ClipPolygonWithLine(polyshp,lineshp,newsf);
                                bool boolresult = MapWinGeoProc.SpatialOperations.ClipPolygonWithLine(ref polyshp,ref lineshp,out newsf);    //SectionPolygonWithLine(ref lineshp, ref polyshp, (int)0, (int)2, ref newsf);
                                if (boolresult)
                                {
                                    int nextpinsw = 0;

                                    for (int ii = 0; ii < newsf.NumShapes; ii++)
                                    {
                                        int newshpidx = sf.NumShapes;
                                        MapWinGIS.Shape sh = newsf.get_Shape(ii);
                                        sf.StartEditingShapes(true, null);
                                        if (sf.EditInsertShape(sh, ref newshpidx) == false)
                                        {
                                            MessageBox.Show("Failed to add the new shape to the shapefile.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            sf.StopEditingShapes(false, true, null);
                                            return;
                                        }
                                        else
                                        {
                                            if (sf.EditingShapes == true)
                                            {
                                                mothershpindxcoll.Add(intmotheridx);
                                                newshpindxcoll.Add(newshpidx);
                                                for (int fldidx = 0; fldidx < sf.NumFields; fldidx++)
                                                {
                                                    if (sf.get_Field(fldidx).Name.ToUpper() != "SHAPE_ID" || sf.get_Field(fldidx).Name.ToUpper() != "")
                                                    {
                                                        object sval = sf.get_CellValue(fldidx, Convert.ToInt32(shapes.GetValue(i)));
                                                        if (sf.Table.get_Field(fldidx).Type == FieldType.STRING_FIELD)
                                                        {
                                                            if (sval == null)
                                                            {
                                                                sval = (String)"";
                                                            }
                                                            else
                                                            {
                                                                //string ssect = String.Empty;
                                                                if (sf.get_Field(fldidx).Name == "PIN")
                                                                {
                                                                    if (lastpin != String.Empty)
                                                                    {
                                                                        if (sval.ToString().Substring(0, 14) != lastpin.Substring(0, 14))
                                                                            nextpinsw = 0;
                                                                    }
                                                                    if (nextpinsw == 0)
                                                                    {
                                                                        if (sval.ToString() == "")
                                                                        {

                                                                            //get dbase faas_land table last parcel pin of the sect
                                                                            //based on the selected section boundary e.g. 002-05-001-001-099
                                                                            //not yet tested
                                                                            lastpin = GetPinFromSectBndry(ext, "SECTION BOUNDARY");
                                                                            lastpin = GetDBaseLastPin(lastpin, dsnOLEDB);
                                                                            nextpinsw = 1;
                                                                            //lastpin = sval.ToString();
                                                                        }
                                                                        else
                                                                        {
                                                                            //get dbase faas_land table last parcel pin of the sect
                                                                            //based on the selected parcel boundary e.g. 001
                                                                            lastpin = GetDBaseLastPin(sval.ToString(), dsnOLEDB);
                                                                            //if (sval.ToString().Length == 18 && sval.ToString() != "")
                                                                            //{
                                                                            //    //double result;
                                                                            //    //if (double.TryParse(ssect, out result) == true)
                                                                            //    //{
                                                                            //    //sval = sval.ToString().Substring(0, 15) + String.Format("{0:D3}", Convert.ToInt32(ssect) + 1);
                                                                            nextpinsw = 1;
                                                                            //    //lastpin = sval.ToString();
                                                                            //    //}
                                                                            //}
                                                                        }
                                                                        //the last dbase pin
                                                                        sval = lastpin;
                                                                        //sval = lastpin.Substring(0, 15) + String.Format("{0:D3}", Convert.ToInt32(lastpin.Substring(15, 3)));
                                                                        //get gis shapefile table last parcel pin of the section
                                                                        //based on the selected land parcel boundary
                                                                        // try to validate if the new suggested pin from the database 
                                                                        // are already existing in the gis land parcel shapefile
                                                                        string error = "";
                                                                        object result = null;
                                                                        if (sf.Table.Query("[PIN] = \"" + sval.ToString() + "\"", ref result, ref error))
                                                                        {
                                                                            int[] shapes2 = result as int[];
                                                                            if (shapes2 != null)
                                                                            {
                                                                                //the suggested pin from the dbase is already existing in the shapefile
                                                                                //get new suggested pin from the gis shapefile
                                                                                shapes2 = null;
                                                                                string error2 = "";
                                                                                object result2 = null;
                                                                                StringBuilder sqry = new StringBuilder();
                                                                                sqry.Append(string.Format("[BRGY] = \"{0}\"", sval.ToString().Substring(7, 3)));
                                                                                sqry.Append(string.Format(" AND [SECT] = \"{0}\"", sval.ToString().Substring(11, 3)));
                                                                                sqry.Append(string.Format(" AND [PRCL] <> \"{0}\"", "999"));
                                                                                if (sf.Table.Query(sqry.ToString(), ref result2, ref error2))
                                                                                {
                                                                                    shapes2 = (int[])result2;
                                                                                    ArrayList prclColl = new ArrayList();
                                                                                    int intprcl;
                                                                                    for (int iii = 0; iii < shapes2.Length; iii++)
                                                                                    {
                                                                                        int fidx = -1;
                                                                                        fidx = sf.Table.get_FieldIndexByName("PRCL");
                                                                                        if (fidx != -1)
                                                                                        {
                                                                                            prclColl.Add(sf.get_CellValue(fidx, Convert.ToInt32(shapes2.GetValue(iii).ToString())).ToString());
                                                                                        }
                                                                                    }
                                                                                    prclColl.Sort();
                                                                                    prclColl.Reverse();
                                                                                    if (int.TryParse(prclColl[0].ToString(), out intprcl))
                                                                                        lastpin = sval.ToString().Substring(0, 15) + String.Format("{0:D3}", Convert.ToInt32(prclColl[0].ToString()));
                                                                                    //lastpin = sval.ToString();
                                                                                }
                                                                                else
                                                                                {
                                                                                    lastpin = sval.ToString();
                                                                                    //    //if the validation is false or no records were found in the shapefile
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            //test if the dbase suggested pin is the greater than the last pin in the shapefile
                                                                            //dbase pin vs shapefile pin
                                                                            int dbasepin = -1;
                                                                            int shapepin = -1;
                                                                            if (IsNumeric(sval.ToString().Substring(15, 3)))
                                                                                dbasepin = Convert.ToInt32(sval.ToString().Substring(15, 3));

                                                                            //get shapefile last prcl of sect
                                                                            StringBuilder sqry2 = new StringBuilder();
                                                                            sqry2.Append(string.Format("[BRGY] = \"{0}\"", sval.ToString().Substring(7, 3)));
                                                                            sqry2.Append(string.Format(" AND [SECT] = \"{0}\"", sval.ToString().Substring(11, 3)));
                                                                            sqry2.Append(string.Format(" AND [PRCL] <> \"{0}\"", "999"));
                                                                            if (sf.Table.Query(sqry2.ToString(), ref result, ref error))
                                                                            {
                                                                                int[] shapes3 = result as int[];
                                                                                ArrayList prclColl2 = new ArrayList();
                                                                                int intprcl;
                                                                                for (int iii = 0; iii < shapes3.Length; iii++)
                                                                                {
                                                                                    int fidx = -1;
                                                                                    fidx = sf.Table.get_FieldIndexByName("PRCL");
                                                                                    if (fidx != -1)
                                                                                        prclColl2.Add(sf.get_CellValue(fidx, Convert.ToInt32(shapes3.GetValue(iii).ToString())).ToString());
                                                                                }
                                                                                prclColl2.Sort();
                                                                                prclColl2.Reverse();
                                                                                if (int.TryParse(prclColl2[0].ToString(), out intprcl))
                                                                                    shapepin = Convert.ToInt32(prclColl2[0].ToString());
                                                                                //sval = lastpin.Substring(0, 15) + String.Format("{0:D3}", Convert.ToInt32(prclColl[0].ToString()) + 1);

                                                                            }

                                                                            if (shapepin > dbasepin)
                                                                                lastpin = sval.ToString().Substring(0, 15) + String.Format("{0:D3}", shapepin);
                                                                            else
                                                                                lastpin = sval.ToString().Substring(0, 15) + String.Format("{0:D3}", dbasepin);

                                                                            //lastpin = sval.ToString();
                                                                        }
                                                                    }
                                                                    sval = lastpin.Substring(0, 15) + String.Format("{0:D3}", Convert.ToInt32(lastpin.Substring(15, 3)) + 1);
                                                                    lastpin = sval.ToString();
                                                                }
                                                                else if (sf.get_Field(fldidx).Name == "CITY")
                                                                    sval = lastpin.ToString().Substring(0, 3);
                                                                else if (sf.get_Field(fldidx).Name == "DIST")
                                                                    sval = lastpin.ToString().Substring(4, 2);
                                                                else if (sf.get_Field(fldidx).Name == "BRGY")
                                                                    sval = lastpin.ToString().Substring(7, 3);
                                                                else if (sf.get_Field(fldidx).Name == "SECT")
                                                                    sval = lastpin.ToString().Substring(11, 3);
                                                                else if (sf.get_Field(fldidx).Name == "PRCL")
                                                                    sval = lastpin.ToString().Substring(15, 3);
                                                            }
                                                        }
                                                        else if (sf.Table.get_Field(fldidx).Type == FieldType.DOUBLE_FIELD)
                                                        {
                                                            if (sval == null)
                                                            {
                                                                sval = (double)0;
                                                            }
                                                            else
                                                            {
                                                                //pending work to verify the area and perimeter of decimal degree projection
                                                                if (sf.get_Field(fldidx).Name.ToUpper() == "AREA")
                                                                {

                                                                    double dbArea = MapWinGeoProc.Utils.Area(ref sh, MapWindow.Interfaces.UnitOfMeasure.DecimalDegrees);
                                                                    dbArea = MapWinGeoProc.UnitConverter.ConvertArea(MapWindow.Interfaces.UnitOfMeasure.Kilometers, MapWindow.Interfaces.UnitOfMeasure.Meters, dbArea);
                                                                    sval = dbArea;//sval = sh.Area;
                                                                }
                                                                else if (sf.get_Field(fldidx).Name.ToUpper() == "PERIMETER")
                                                                {
                                                                    Utils util = new Utils();
                                                                    double dbPerimeter = util.get_Perimeter(sh);
                                                                    dbPerimeter = MapWinGeoProc.UnitConverter.ConvertLength(MapWindow.Interfaces.UnitOfMeasure.Kilometers, MapWindow.Interfaces.UnitOfMeasure.Meters, dbPerimeter);
                                                                    sval = dbPerimeter;//sval = sh.Perimeter;
                                                                }
                                                            }
                                                        }
                                                        else if (sf.Table.get_Field(fldidx).Type == FieldType.INTEGER_FIELD)
                                                        {
                                                            if (sval == null)
                                                            {
                                                                sval = (int)0;
                                                            }
                                                            else
                                                            {
                                                                if (sf.get_Field(fldidx).Name.ToUpper() == "CORNER_LOT")
                                                                    sval = (int)0;
                                                            }
                                                        }
                                                        sf.EditCellValue(fldidx, newshpidx, sval);
                                                    }
                                                }
                                                //create a backup of shapefile here before saving anything
                                                sf.StopEditingShapes(true, true, null);
                                            }
                                        }
                                    }
                                }
                                //else
                                    //MessageBox.Show("wa");
                            }
                            catch
                            {
                                continue;
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message.ToString(), frmgooglekml.Text);

            }
        }

        public void VerifyShape7(MapWinGIS.Shape s, string dsnOLEDB)
        {
            ArrayList newshpindxcoll = new ArrayList();//edited 10/17/2013
            ArrayList mothershpindxcoll = new ArrayList();//edited 10/17/2013
            if (frmsubd != null)
            {
                if (frmsubd.Visible == false)
                {
                    newshpindxcoll = new ArrayList();
                    mothershpindxcoll = new ArrayList();
                }
            }
            //ArrayList newshpindxcoll = new ArrayList();////remarked 9/17/2013
            //ArrayList mothershpindxcoll = new ArrayList();////remarked 9/17/2013
            string strmotherpin = String.Empty;
            int intmotheridx = -1;
            if (s.numPoints < 2)
            {
                resetLine("SUBDLAND");
                Logger.Message("You must add at least two points for a line.", "Not Enough Points", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, DialogResult.OK);
                return;
            }

            Shapefile newsf = new MapWinGIS.Shapefile();
            if (System.IO.File.Exists("new_temp_subd_shp.shp"))
                System.IO.File.Delete("new_temp_subd_shp.shp");
            if (System.IO.File.Exists("new_temp_subd_shp.dbf"))
                System.IO.File.Delete("new_temp_subd_shp.dbf");
            if (System.IO.File.Exists("new_temp_subd_shp.shx"))
                System.IO.File.Delete("new_temp_subd_shp.shx");
            if (System.IO.File.Exists("new_temp_subd_shp.prj"))
                System.IO.File.Delete("new_temp_subd_shp.prj");

            bool results = newsf.CreateNew("C:\\new_temp_subd_shp.shp", MapWinGIS.ShpfileType.SHP_POLYGON);
            object ShapeIDS = new object();
            Shapefile sf = (Shapefile)frmM.axMap.get_GetObject(frmM.hndMapSel);
            Extents ext = frmM.m_newlineshape.Extents;

            try
            {
                if (sf.SelectShapes(ext, (double)0, SelectMode.INTERSECTION, ref ShapeIDS))
                {
                    int[] shapes = (int[])ShapeIDS;
                    if (shapes.Length > 0)
                    {
                        string lastpin = String.Empty;
                        for (int i = 0; i < shapes.Length; i++)
                        {
                            MapWinGIS.Shape polyshp = sf.get_Shape(Convert.ToInt32(shapes.GetValue(i)));
                            MapWinGIS.Shape lineshp = frmM.m_newlineshape;
                            strmotherpin = sf.Table.get_CellValue(sf.Table.get_FieldIndexByName("PIN"), Convert.ToInt32(shapes.GetValue(i))).ToString();
                            intmotheridx = Convert.ToInt32(shapes.GetValue(i));
                            try
                            {
                                for (int x = 0; x < polyshp.NumParts; x++)
                                {
                                }

                                //if (clscommon.Accurate_ClipPolygonWithLine(ref polyshp, ref lineshp, out newsf))
                                //{ 
                                //    for (int ii = 0; ii < newsf.NumShapes; ii++)
                                //    {
                                //        int newshpidx = sf.NumShapes;
                                //        MapWinGIS.Shape sh = newsf.get_Shape(ii);
                                //    }
                                //}

                                bool boolresult = MapWinGeoProc.SpatialOperations.ClipPolygonWithLine(ref polyshp,ref lineshp,out newsf);    //SectionPolygonWithLine(ref lineshp, ref polyshp, (int)0, (int)2, ref newsf);
                                if (boolresult)
                                {
                                    int nextpinsw = 0;

                                    for (int ii = 0; ii < newsf.NumShapes; ii++)
                                    {
                                        int newshpidx = sf.NumShapes;
                                        MapWinGIS.Shape sh = newsf.get_Shape(ii);
                                    }
                                }

                            }
                            catch
                            {
                                //continue;
                            }
                        }

                        //newsf.StopEditingShapes(true
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message.ToString(), frmgooglekml.Text);

            }
        }

        public bool verifyUserRights(string sUser, string sRightCode, string sConnString)
        {
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                OleDbConnection oOleDbConnection = new OleDbConnection(sConnString);
                string sQuery = "select distinct userid,right_code from gis_user_rights where userid = '" + sUser + "' and right_code = '" + sRightCode + "'";
                OleDbCommand cmd = new OleDbCommand(sQuery, oOleDbConnection);
                OleDbDataAdapter newDataAdapter = new OleDbDataAdapter(cmd);
                oOleDbConnection.Open();
                OleDbDataReader newDataReaders = cmd.ExecuteReader();
                bool ans = false;
                if (newDataReaders.HasRows == true)
                {
                    ans = true;
                }
                else if (newDataReaders.HasRows == false)
                {
                    ans = false;
                }
                newDataReaders.Close();
                oOleDbConnection.Close();
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                return ans;
            }
            catch
            {
                return false;
            }
        }

        public void cornerLotLegend()
        {
            Cursor.Current = Cursors.WaitCursor;
            setclickdefault();

            Shapefile sf = (Shapefile)frmM.axMap.get_GetObject(frmM.hndMapSel);

            Table sftable = sf.Table;
            int fldidx = -1;

            if (sf.ShapefileType == ShpfileType.SHP_POLYGON)
            {
                if ((frmM.legend1.Map.get_LayerName(frmM.hndMapSel).ToUpper().Contains("BARANGAY")) && (frmM.legend1.Map.get_LayerName(frmM.hndMapSel).ToUpper() != ("BARANGAY BOUNDARY")))
                {
                    ////to do set the fieldnames as dymanic
                    fldidx = sftable.get_FieldIndexByName("CORNER_LOT");
                    //string strval = String.Empty;
                    //if (fldidx != -1)
                    //{
                    //    if (sf.Table.NumRows > 0)
                    //    {
                    //        int cntr = 0;
                    //        do
                    //        {
                    //            strval = sf.get_CellValue(fldidx, cntr).ToString().Trim();
                    //            cntr += 1;
                    //            if (sf.Table.NumRows == cntr)
                    //                break;
                    //        } while (sf.get_CellValue(fldidx, cntr).ToString().Trim().Length > 0);
                    //    }
                    //}
                    //to do set the fieldnames as dymanic
                    int pinfldidx = sftable.get_FieldIndexByName("PIN");
                    string strpinval = String.Empty;
                    if (pinfldidx != -1)
                    {
                        if (sf.Table.NumRows > 0)
                        {
                            int cntr = 0;
                            do
                            {
                                strpinval = sf.get_CellValue(pinfldidx, cntr).ToString().Trim();
                                cntr += 1;
                                if (sf.Table.NumRows == cntr)
                                    break;
                            } while (sf.get_CellValue(pinfldidx, cntr).ToString().Trim().Length > 0);
                            frmM.m_layerbrgy = strpinval.Substring(7, 3);
                            frmM.m_layercity = strpinval.Substring(0, 3);
                            frmM.m_layerdist = strpinval.Substring(4, 2);
                        }

                    }

                    //set new field and get values from database
                    bool result = sf.StartEditingShapes(true, null);
                    if (fldidx == -1)
                    {
                        MapWinGIS.Field fld = new MapWinGIS.FieldClass();
                        fld.Key = "CORNER_LOT";
                        fld.Name = "CORNER_LOT";
                        fld.Width = 1;
                        fld.Type = MapWinGIS.FieldType.STRING_FIELD;
                        int intref = sftable.NumFields;
                        sf.EditInsertField(fld, ref intref, null);
                        fldidx = sftable.get_FieldIndexByName("CORNER_LOT");
                    }
                    StringBuilder sqry = new StringBuilder();
                    sqry.Append("SELECT x1.land_pin FROM gis_corner_lot x1");
                    sqry.Append(String.Format(" WHERE x1.land_pin like '{0}%'", frmM.m_layercity + "-" + frmM.m_layerdist + "-" + frmM.m_layerbrgy));
                    sqry.Append(" ORDER BY x1.land_pin");

                    OleDbConnection oledbconn = new OleDbConnection(frmM.m_strconn);
                    OleDbCommand cmd = new OleDbCommand(sqry.ToString(), oledbconn);
                    OleDbDataAdapter newDataAdapter = new OleDbDataAdapter(cmd);
                    DataSet newDataSet = new DataSet();
                    oledbconn.Open();
                    OleDbDataReader newDataReaders = cmd.ExecuteReader();

                    string strval = String.Empty;
                    if (newDataReaders.HasRows == true)
                    {
                        for (int i = 0; i < sf.NumShapes; i++)
                        {
                            strval = "0";
                            sf.EditCellValue(fldidx, i, strval);
                        }
                        while (newDataReaders.Read())
                        {
                            strval = String.Empty;
                            string strPinNo = String.Empty;
                            int shpidx = -1;
                            if (!DBNull.Value.Equals(newDataReaders["LAND_PIN"]))
                            {
                                strPinNo = newDataReaders["LAND_PIN"].ToString().Trim().ToUpper();
                            }
                            for (int i = 0; i < sf.NumShapes; i++)
                            {
                                if (strPinNo == sf.get_CellValue(pinfldidx, i).ToString().ToUpper().Trim())
                                {
                                    shpidx = i;
                                    break;
                                }
                            }
                            if (!DBNull.Value.Equals(newDataReaders["LAND_PIN"]))
                            {
                                if (shpidx > -1)
                                {
                                    strval = "1";
                                    if (strval != String.Empty)
                                        sf.EditCellValue(fldidx, shpidx, strval);
                                }
                            }
                        }
                    }
                    result = sf.StopEditingShapes(true, true, null);

                    sf.DefaultDrawingOptions.FillVisible = true;
                    sf.Categories.Clear();

                    //set category
                    //for setting the fill values
                    sf.DefaultDrawingOptions.FillTransparency = (float)255;
                    sf.DefaultDrawingOptions.FillType = tkFillType.ftStandard; //tkFillType.ftGradient;
                    sf.DefaultDrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Black));
                    sf.DefaultDrawingOptions.FillBgTransparent = false;
                    //for setting the outline values
                    sf.DefaultDrawingOptions.LineColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.LimeGreen));
                    sf.DefaultDrawingOptions.LineTransparency = (float)255;
                    sf.DefaultDrawingOptions.LineWidth = 1;

                    sf.Categories.Generate(fldidx, tkClassificationType.ctUniqueValues, 0);

                    for (int i = 0; i < sf.Categories.Count; i++)
                    {
                        ShapefileCategory cat = sf.Categories.get_Item(i);
                        if (cat.Name.Trim() == "")
                        {
                            cat.DrawingOptions.FillVisible = false;
                            cat.DrawingOptions.FillColor = (uint)System.Drawing.ColorTranslator.ToOle(Color.Black);
                            cat.DrawingOptions.LineVisible = true;
                            cat.DrawingOptions.LineColor = (uint)System.Drawing.ColorTranslator.ToOle(Color.LimeGreen);
                            cat.Name = "NO VALUE";
                        }
                        else if (cat.Name.Trim() == "1")
                        {
                            cat.DrawingOptions.FillVisible = true;
                            cat.DrawingOptions.FillColor = (uint)System.Drawing.ColorTranslator.ToOle(Color.Orange);
                            cat.DrawingOptions.LineVisible = true;
                            cat.DrawingOptions.LineColor = (uint)System.Drawing.ColorTranslator.ToOle(Color.LimeGreen);
                            cat.Name = "CORNER LOT";
                        }
                        else if (cat.Name.Trim() == "0")
                        {
                            cat.DrawingOptions.FillVisible = true;
                            cat.DrawingOptions.FillColor = (uint)System.Drawing.ColorTranslator.ToOle(Color.Gray);
                            cat.DrawingOptions.LineVisible = true;
                            cat.DrawingOptions.LineColor = (uint)System.Drawing.ColorTranslator.ToOle(Color.LimeGreen);
                            cat.Name = "NON CORNER LOT";
                        }
                    }
                    sf.Categories.Caption = "LAND: CORNER LOT";
                    sf.DefaultDrawingOptions.LineVisible = true;
                    sf.DefaultDrawingOptions.VerticesVisible = false;
                    sf.DefaultDrawingOptions.FillVisible = false;

                    frmM.legend1.Layers.ItemByHandle(frmM.hndMapSel).Refresh();
                    frmM.axMap.Redraw();
                    frmM.axMap.Refresh();

                    Cursor.Current = Cursors.Default;
                }
            }
        }

        public void initTbl(string sConnString)
        {
            //check if the gis_a_trail_parcel table exist
            string stable1 = "GIS_A_TRAIL_PARCEL";
            StringBuilder squery = new StringBuilder();
            squery.Append(String.Format("select count(*) from user_tables where table_name= '{0}'", stable1));
            string strtblcnt = returnValue(squery.ToString(), sConnString, stable1);
            if (strtblcnt == "" || strtblcnt == "0" || strtblcnt == null)
            {
                squery = new StringBuilder();
                squery.Append(String.Format("CREATE TABLE \"{0}\".\"{1}\"\n", frmM.m_strUserId.ToUpper(), stable1));
                squery.Append("(\n");
                squery.Append("\"USERID\" VARCHAR2(4000 BYTE),\n");
                squery.Append("\"TR_DATE\" DATE,\n");
                squery.Append("\"MODULE_CODE\" VARCHAR2(4000 BYTE),\n");
                squery.Append("\"AFF_TABLE_LAYER\" VARCHAR2(4000 BYTE),\n");
                squery.Append("\"COMPUTER_NAME\" VARCHAR2(4000 BYTE),\n");
                squery.Append("\"TR_DETAILS\" VARCHAR2(4000 BYTE)\n");
                squery.Append(")");

                updateDBASE(sConnString, squery.ToString());
            }
        }

        public double Deg2RadPrcl(string Deg, string Min)
        {
            string Sec = "0";
            double Deg2Dec = (Convert.ToDouble(Convert.ToDouble(Deg)) + (Double)(Convert.ToDouble(Min) / 60) + (Double)(Convert.ToDouble(Sec) / 3600)) * Math.PI / 180.0;
            return Deg2Dec;
        }

        public double Deg2Rad(String Deg, String Min, String Sec)
        {
            double Deg2Dec = (Convert.ToDouble(Convert.ToDouble(Deg)) + (Double)(Convert.ToDouble(Min) / 60) + (Double)(Convert.ToDouble(Sec) / 3600)) * Math.PI / 180.0;
            return Deg2Dec;
        }

        public double ComputeAngle2(double P1x, double P1y, double P2x, double P2y)
        {
            //double dX;
            //double dY;
            //double theta = 0.0;
            //double angle = 0.0;

            double dX = P2x - P1x;
            double dY = P2y - P1y;
            double dxy = dY / dX;
            double theta = Math.Atan(dxy);
            double angle = ((theta * 180) / Math.PI);
            //double angle = Math.Atan2(dY, dX) * (180 / Math.PI);
            return angle;
        }

        public void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            int subdrtorycntr = 0;
            int filecntr = 0;
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the source directory does not exist, throw an exception.
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
            }

            // If the destination directory does not exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }


            // Get the file contents of the directory to copy.
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                filecntr += 1;
                // Create the path to the new copy of the file.
                string temppath = Path.Combine(destDirName, file.Name);

                // Copy the file.
                file.CopyTo(temppath, false);
                //if (subdrtorycntr == dirs.Length && filecntr == files.Length)
                //{
                //    MessageBox.Show("liloy");
                //}
            }

            // If copySubDirs is true, copy the subdirectories.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    subdrtorycntr += 1;
                    // Create the subdirectory.
                    string temppath = Path.Combine(destDirName, subdir.Name);

                    // Copy the subdirectories.
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        public void fileCopy(string sourcePath, string targetPath, string shpfilenm)
        {
            string[] files = System.IO.Directory.GetFiles(sourcePath);

            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            string fileName = String.Empty;
            string destFile = String.Empty;
            // Copy the files and overwrite destination files if they already exist. 
            foreach (string s in files)
            {
                // Use static Path methods to extract only the file name from the path.
                if (s.Contains(shpfilenm) && (Path.GetFileNameWithoutExtension(s).ToUpper()==shpfilenm.ToUpper()))
                {
                    fileName = System.IO.Path.GetFileName(s);
                    destFile = System.IO.Path.Combine(targetPath, fileName);
                    System.IO.File.Copy(s, destFile, true);
                }
            }
        }

        public void gisuserdbasetables(frmMain frm)
        {
            frmM = frm;
            //check if the GIS_USERS table exist
            string stable = "GIS_USERS";
            StringBuilder sqry = new StringBuilder();
            sqry.Append(String.Format("select count(*) from user_tables where table_name= '{0}'", stable));
            string strtblcnt = returnValue(sqry.ToString(), frmM.m_strconn, stable);
            if (strtblcnt == "" || strtblcnt == "0" || strtblcnt == null)
            {
                sqry = new StringBuilder();
                sqry.Append(String.Format("CREATE TABLE \"{0}\".\"{1}\"\n", frmM.m_strUserId.ToUpper(), stable));
                sqry.Append("(\n");
                sqry.Append("\"USERID\" VARCHAR2(50 BYTE),\n");
                sqry.Append("\"PASSWORD\" VARCHAR2(500 BYTE),\n");
                sqry.Append("\"USERNAME\" VARCHAR2(50 BYTE),\n");
                sqry.Append("\"DESIGNATION\" VARCHAR2(50 BYTE), \n");
                sqry.Append("\"USER_AUTHORIZATION\" VARCHAR2(50 BYTE),\n");
                sqry.Append("\"LASTNAME\" VARCHAR2(50 BYTE),\n");
                sqry.Append("\"FIRSTNAME\" VARCHAR2(50 BYTE),\n");
                sqry.Append("\"MIDDLENAME\" VARCHAR2(50 BYTE),\n");
                sqry.Append("\"SYSTEM_CODE\" VARCHAR2(2 BYTE) NOT NULL ENABLE\n");
                sqry.Append(")");
                updateDBASE(frmM.m_strconn, sqry.ToString());

                sqry = new StringBuilder();
                sqry.Append("Insert into GIS_USERS (USERID,PASSWORD,USERNAME,DESIGNATION,USER_AUTHORIZATION,LASTNAME,FIRSTNAME,MIDDLENAME,SYSTEM_CODE) values ('ADMIN','727ee8b77c2350bc','ADMIN','GIS ADMINISTRATOR','ADMINISTRATOR','SOLUTIONS','AMELLAR','AS','1')");
                updateDBASE(frmM.m_strconn, sqry.ToString());
            }

            //check if the GIS_USER_RIGHTS table exist
            stable = "GIS_USER_RIGHTS";
            sqry = new StringBuilder();
            sqry.Append(String.Format("select count(*) from user_tables where table_name= '{0}'", stable));
            strtblcnt = returnValue(sqry.ToString(), frmM.m_strconn, stable);
            if (strtblcnt == "" || strtblcnt == "0" || strtblcnt == null)
            {
                sqry = new StringBuilder();
                sqry.Append(String.Format("CREATE TABLE \"{0}\".\"{1}\"\n", frmM.m_strUserId.ToUpper(), stable));
                sqry.Append("(\n");
                sqry.Append("\"USERID\" VARCHAR2(4000 BYTE),\n");
                sqry.Append("\"RIGHT_CODE\" VARCHAR2(4000 BYTE)\n");
                sqry.Append(")");
                updateDBASE(frmM.m_strconn, sqry.ToString());

                sqry = new StringBuilder();
                sqry.Append("Insert all\n");
                sqry.Append("into GIS_USER_RIGHTS (USERID,RIGHT_CODE) values ('ADMIN','GRANT_ACNT')\n");
                sqry.Append("into GIS_USER_RIGHTS (USERID,RIGHT_CODE) values ('ADMIN','LOCK_ACNT')\n");
                sqry.Append("into GIS_USER_RIGHTS (USERID,RIGHT_CODE) values ('ADMIN','USR_ACNT')\n");
                sqry.Append("Select * from dual");
                updateDBASE(frmM.m_strconn, sqry.ToString());
            }

            //check if the GIS_USER_LEVEL_RIGHTS table exist
            stable = "GIS_USER_LEVEL_RIGHTS";
            sqry = new StringBuilder();
            sqry.Append(String.Format("select count(*) from user_tables where table_name= '{0}'", stable));
            strtblcnt = returnValue(sqry.ToString(), frmM.m_strconn, stable);
            if (strtblcnt == "" || strtblcnt == "0" || strtblcnt == null)
            {
                sqry = new StringBuilder();
                sqry.Append(String.Format("CREATE TABLE \"{0}\".\"{1}\"\n", frmM.m_strUserId.ToUpper(), stable));
                sqry.Append("(\n");
                sqry.Append("\"USER_LEVEL\" VARCHAR2(4000 BYTE),\n");
                sqry.Append("\"MODULE_CODE\" VARCHAR2(4000 BYTE),\n");
                sqry.Append("\"MODULE_DESC\" VARCHAR2(4000 BYTE),\n");
                sqry.Append("\"SYSTEM_CODE\" VARCHAR2(4000 BYTE)\n");
                sqry.Append(")");
                updateDBASE(frmM.m_strconn, sqry.ToString());

                sqry = new StringBuilder();
                sqry.Append("Insert all\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','DELINQ','DELINQUENCY THEMATIC REPORT','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','LOCK_ACNT','LOCKED ACCOUNT','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','GRANT_ACNT','USER GRANT','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','USR_ACNT','USERS ACCOUNT','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','ZOOMING','MAP ZOOM CONTROLS','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','LABELLING','MAP LABELLING TOOL CONTROLS','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','LAND_LOAD','LOAD LAND PARCEL LAYER PER BARANGAY','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','LAND_ATTACHIMAGE','ATTACHED SCANNED IMAGES TO THE PARCEL','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','INFO','ACCESS TO DATABASE INFORMATION','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','LAND_THEAMATIC','CHANGES LAND PARCEL THEAMATIC BY CATEGORY','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','BLDG_LOAD','LOAD BUILDING LAYER PER BARANGAY','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','BLDG_THEAMATIC','CHANGES BUILDING THEAMATIC BY CATEGORY','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','PARCEL_SEARCH','SEARCH TOOL TO FIND LAND PARCEL LOCATIONS','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','LAND_GOOGLE','LAND PARCEL ON GOOGLE OVERLAYED','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','MAP_LAYOUT','CREATE MAP LAYOUT','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','REPORT_TMCR','CREATE TAX MAP CONTROL ROLL REPORT','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','REPORT_MATCHING','CREATE MATCHING REPORT OF GIS PARCELS VS RPTA DATABASE','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','LAND_SUBD','TOOL TO SUBDIVIDE LAND PARCELS','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','LAND_CONS','TOOL TO CONSOLIDATE LAND PARCELS','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','LAND_REPIN','TOOL TO REPIN LAND PARCELS','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','MUN_BNDRY_DRAW','CREATE LGU BOUNDARY FROM TECHNICAL DESCRIPTION','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','PRCL_BNDRY_DRAW','CREATE PARCEL BOUNDARY FROM TECHNICAL DESCRIPTION','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','LANDMARK_LOAD','LOAD LANDMARKS LAYER PER CATEGORY','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','LANDMARK_SEARCH','SEARCH TOOL TO FIND LANDMARK LOCATIONS','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','LANDMARK_ADD','CREATE NEW LANDMARK POINT','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','LANDMARK_DEL','REMOVE EXISTING LANDMARK POINT','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','ROAD_SEARCH','SEARCH TOOL TO FIND STREETS LOCATION','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','ROAD_RENAME','REDEFINE STREET NAMES','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','CORNER_ADD','IDENTIFY CORNER LOTS FROM LAND PARCELS','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','CORNER_DEL','REVOKE LAND PARCELS AS CORNER LOTS','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','REPORT_CORNER','CREATE CORNER LOTS REPORT FROM PARCELS','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','LAYER_PROP','ACCESS TO LAYER PROPERTIES','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','OSM_LAYER','LOAD OPEN STREET MAP LAYER ','1')\n");
                sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','GR_TOOL','TOOLS FOR GENERAL REVISION','1')\n");
                //sqry.Append("into GIS_USER_LEVEL_RIGHTS (USER_LEVEL,MODULE_CODE,MODULE_DESC,SYSTEM_CODE) values ('ADMINISTRATOR','LOAD_EXTERNAL','TOOLS FOR LOADING EXTERNAL DATA SOURCES','1')\n");
                sqry.Append("Select * from dual");
                updateDBASE(frmM.m_strconn, sqry.ToString());
            }

            //check if the GIS_LOGS table exist
            stable = "GIS_LOGS";
            sqry = new StringBuilder();
            sqry.Append(String.Format("select count(*) from user_tables where table_name= '{0}'", stable));
            strtblcnt = returnValue(sqry.ToString(), frmM.m_strconn, stable);
            if (strtblcnt == "" || strtblcnt == "0" || strtblcnt == null)
            {
                sqry = new StringBuilder();
                sqry.Append(String.Format("CREATE TABLE \"{0}\".\"{1}\"\n", frmM.m_strUserId.ToUpper(), stable));
                sqry.Append("(\n");
                sqry.Append("\"COMPUTER_NAME\" VARCHAR2(100 BYTE),\n");
                sqry.Append("\"USER_NAME\" VARCHAR2(100 BYTE),\n");
                sqry.Append("\"USER_AUTHORITY\" VARCHAR2(100 BYTE),\n");
                sqry.Append("\"TIME_IN\" DATE\n");
                sqry.Append(")");
                updateDBASE(frmM.m_strconn, sqry.ToString());
            }

            //check if the GIS_LOGS_TEMP table exist
            stable = "GIS_LOGS_TEMP";
            sqry = new StringBuilder();
            sqry.Append(String.Format("select count(*) from user_tables where table_name= '{0}'", stable));
            strtblcnt = returnValue(sqry.ToString(), frmM.m_strconn, stable);
            if (strtblcnt == "" || strtblcnt == "0" || strtblcnt == null)
            {
                sqry = new StringBuilder();
                sqry.Append(String.Format("CREATE TABLE \"{0}\".\"{1}\"\n", frmM.m_strUserId.ToUpper(), stable));
                sqry.Append("(\n");
                sqry.Append("\"COMPUTER_NAME\" VARCHAR2(100 BYTE),\n");
                sqry.Append("\"USER_NAME\" VARCHAR2(100 BYTE),\n");
                sqry.Append("\"USER_AUTHORITY\" VARCHAR2(100 BYTE),\n");
                sqry.Append("\"TIME_IN\" DATE,\n");
                sqry.Append("\"TIME_OUT\" DATE\n");
                sqry.Append(")");
                updateDBASE(frmM.m_strconn, sqry.ToString());
            }

            //check if the GIS_APPLICATION table exist
            stable = "GIS_APPLICATION";
            sqry = new StringBuilder();
            sqry.Append(String.Format("select count(*) from user_tables where table_name= '{0}'", stable));
            strtblcnt = returnValue(sqry.ToString(), frmM.m_strconn, stable);
            if (strtblcnt == "" || strtblcnt == "0" || strtblcnt == null)
            {
                sqry = new StringBuilder();
                sqry.Append(String.Format("CREATE TABLE \"{0}\".\"{1}\"\n", frmM.m_strUserId.ToUpper(), stable));
                sqry.Append("(\n");
                sqry.Append("\"ID\" NUMBER NOT NULL ENABLE,\n");
                sqry.Append("\"SYSTEM_CODE\" VARCHAR2(1 BYTE) NOT NULL ENABLE,\n");
                sqry.Append("\"SYSTEM_NAME\" VARCHAR2(100 BYTE) NOT NULL ENABLE\n");
                sqry.Append(")");
                updateDBASE(frmM.m_strconn, sqry.ToString());

                sqry = new StringBuilder();
                sqry.Append("Insert into GIS_APPLICATION (ID,SYSTEM_CODE,SYSTEM_NAME) values ('1','1','PARCEL MAPPING')");
                updateDBASE(frmM.m_strconn, sqry.ToString());
            }
        }

        public void gisdbasetablescreate()
        {
            //check if the GIS_REPORTS table exist
            string stable = "GIS_REPORTS";
            StringBuilder sqry = new StringBuilder();
            sqry.Append(String.Format("select count(*) from user_tables where table_name= '{0}'", stable));
            string strtblcnt = returnValue(sqry.ToString(), frmM.m_strconn, stable);
            if (strtblcnt == "" || strtblcnt == "0" || strtblcnt == null)
            {
                sqry = new StringBuilder();
                sqry.Append(String.Format("CREATE TABLE \"{0}\".\"{1}\"\n", frmM.m_strUserId.ToUpper(), stable));
                sqry.Append("(\n");
                sqry.Append("\"ID\"  NUMBER(2,0) NOT NULL ENABLE,\n");
                sqry.Append("\"REPORTNM\" VARCHAR2(100 BYTE),\n");
                sqry.Append("\"LOCATION\" VARCHAR2(225 BYTE),\n");
                sqry.Append("\"FILENM\"  VARCHAR2(100 BYTE),\n");
                sqry.Append("\"COMPUTERNM\"  VARCHAR2(100 BYTE) \n");
                sqry.Append(")");

                updateDBASE(frmM.m_strconn, sqry.ToString());
            }

            ////gis_reports_sqnc
            stable = "GIS_REPORTS_SQNC";
            sqry = new StringBuilder();
            sqry.Append(String.Format("select count(*) from user_sequences where sequence_name= '{0}'", stable));
            strtblcnt = returnValue(sqry.ToString(), frmM.m_strconn, stable);
            if (strtblcnt == "" || strtblcnt == "0" || strtblcnt == null)
            {
                sqry = new StringBuilder();
                sqry.Append(String.Format("CREATE SEQUENCE \"{0}\".\"{1}\" MINVALUE 1 MAXVALUE 999999999999999999999999999 INCREMENT BY 1 START WITH 1", frmM.m_strUserId.ToUpper(), stable));
                updateDBASE(frmM.m_strconn, sqry.ToString());
            }

            ////check if the GIS_LAND_ADJ table exist 
            stable = "GIS_LAND_ADJ";
            sqry = new StringBuilder();
            sqry.Append(String.Format("select count(*) from user_tables where table_name= '{0}'", stable));
            strtblcnt = returnValue(sqry.ToString(), frmM.m_strconn, stable);
            if (strtblcnt == "" || strtblcnt == "0" || strtblcnt == null)
            {
                sqry = new StringBuilder();
                sqry.Append(String.Format("CREATE TABLE \"{0}\".\"{1}\"\n", frmM.m_strUserId.ToUpper(), stable));
                sqry.Append("(\n");
                sqry.Append("\"LAND_PIN\" VARCHAR2(23 BYTE), \n");
                sqry.Append("\"LAND_CLS\" VARCHAR2(100 BYTE), \n");
                sqry.Append("\"LAND_SUBCLS\" VARCHAR2(50 BYTE), \n");
                sqry.Append("\"UNIT_VAL\" NUMBER(10,0), \n");
                sqry.Append("\"LAND_ADJ_DESC\" VARCHAR2(100 BYTE), \n");
                sqry.Append("\"GR_CODE\" VARCHAR2(3 BYTE), \n");
                sqry.Append("\"USERID\" VARCHAR2(50 BYTE)");
                sqry.Append(")");

                updateDBASE(frmM.m_strconn, sqry.ToString());
            }

            ////for revision of new work feb 11, 2014
            //////check if the GIS_CORNER_LOT_TEMP table exist 
            //stable = "GIS_CORNER_LOT_TEMP";
            //sqry = new StringBuilder();
            //sqry.Append(String.Format("select count(*) from user_tables where table_name= '{0}'", stable));
            //strtblcnt = returnValue(sqry.ToString(), frmM.m_strconn, stable);
            //if (strtblcnt == "" || strtblcnt == "0" || strtblcnt == null)
            //{
            //    sqry = new StringBuilder();
            //    sqry.Append(String.Format("CREATE TABLE \"{0}\".\"{1}\"\n", frmM.m_strUserId.ToUpper(), stable));
            //    sqry.Append("(\n");
            //    sqry.Append("\"LAND_PIN\" VARCHAR2(23 BYTE) \n");
            //    sqry.Append(")");

            //    updateDBASE(frmM.m_strconn, sqry.ToString());
            //}

            ////for revision  feb 11 2014 added userid for the table
            //check if the GIS_LAYERS_ table exist
            //stable = "GIS_LAYERS_" + frmM.m_userid;
            stable = "GIS_LAYERS";
            sqry = new StringBuilder();
            sqry.Append(String.Format("select count(*) from user_tables where table_name= '{0}'", stable));
            strtblcnt = returnValue(sqry.ToString(), frmM.m_strconn, stable);
            if (strtblcnt == "" || strtblcnt == "0" || strtblcnt == null)
            {
                sqry = new StringBuilder();
                sqry.Append(String.Format("CREATE TABLE \"{0}\".\"{1}\"\n", frmM.m_strUserId.ToUpper(), stable));
                sqry.Append("(\n");
                sqry.Append("\"ID\"  NUMBER(5,0) NOT NULL ENABLE,\n");
                sqry.Append("\"LAYER_NAME\" VARCHAR2(100 BYTE),\n");
                sqry.Append("\"FILENM\"  VARCHAR2(100 BYTE),\n");
                sqry.Append("\"LOCATION\" VARCHAR2(225 BYTE),\n");
                sqry.Append("\"CATEGORY\" VARCHAR2(100 BYTE),\n");
                sqry.Append("\"LABEL\" VARCHAR2(100 BYTE),\n");
                sqry.Append("\"USERID\" VARCHAR2(100 BYTE)");
                sqry.Append(")");

                updateDBASE(frmM.m_strconn, sqry.ToString());
            }

            //check if the GIS_LAYERS_SEQUENCE table exist
            stable = "GIS_LAYERS_SQNC";
            sqry = new StringBuilder();
            sqry.Append(String.Format("select count(*) from user_sequences where sequence_name= '{0}'", stable));
            strtblcnt = returnValue(sqry.ToString(), frmM.m_strconn, stable);
            if (strtblcnt == "" || strtblcnt == "0" || strtblcnt == null)
            {
                sqry = new StringBuilder();
                sqry.Append(String.Format("CREATE SEQUENCE \"{0}\".\"{1}\" MINVALUE 1 MAXVALUE 999999999999999999999999999 INCREMENT BY 1 START WITH 1", frmM.m_strUserId.ToUpper(), stable));
                updateDBASE(frmM.m_strconn, sqry.ToString());
            }

            //check if the GIS_A_TRAIL_PARCEL table exist
            stable = "GIS_A_TRAIL_PARCEL";
            sqry = new StringBuilder();
            sqry.Append(String.Format("select count(*) from user_tables where table_name= '{0}'", stable));
            strtblcnt = returnValue(sqry.ToString(), frmM.m_strconn, stable);
            if (strtblcnt == "" || strtblcnt == "0" || strtblcnt == null)
            {
                sqry = new StringBuilder();
                sqry.Append(String.Format("CREATE TABLE \"{0}\".\"{1}\"\n", frmM.m_strUserId.ToUpper(), stable));
                sqry.Append("(\n");
                sqry.Append("\"USERID\" VARCHAR2(4000 BYTE),\n");
                sqry.Append("\"TR_DATE\" DATE,\n");
                sqry.Append("\"MODULE_CODE\" VARCHAR2(4000 BYTE),\n");
                sqry.Append("\"AFF_TABLE_LAYER\" VARCHAR2(4000 BYTE),\n");
                sqry.Append("\"COMPUTER_NAME\" VARCHAR2(4000 BYTE),\n");
                sqry.Append("\"TR_DETAILS\" VARCHAR2(4000 BYTE)\n");
                sqry.Append(")");

                updateDBASE(frmM.m_strconn, sqry.ToString());
            }

            //check if the GIS_RPT_UNMATCH table exist
            stable = "GIS_RPT_UNMATCH";
            sqry = new StringBuilder();
            sqry.Append(String.Format("select count(*) from user_tables where table_name= '{0}'", stable));
            strtblcnt = returnValue(sqry.ToString(), frmM.m_strconn, stable);
            if (strtblcnt == "" || strtblcnt == "0" || strtblcnt == null)
            {
                sqry = new StringBuilder();
                sqry.Append(String.Format("CREATE TABLE \"{0}\".\"{1}\"\n", frmM.m_strUserId.ToUpper(), stable));
                sqry.Append("(\n");
                sqry.Append("\"PIN\" VARCHAR2(18 BYTE) NOT NULL ENABLE\n");
                sqry.Append(")");

                updateDBASE(frmM.m_strconn, sqry.ToString());
            }

            string stable2 = "GIS_MATCHING_REPORT";
            sqry = new StringBuilder();
            sqry.Append(String.Format("select count(*) from user_tables where table_name= '{0}'", stable2));
            strtblcnt = returnValue(sqry.ToString(), frmM.m_strconn, stable2);
            if (strtblcnt == "" || strtblcnt == "0" || strtblcnt == null)
            {
                sqry = new StringBuilder();
                sqry.Append(String.Format("CREATE TABLE \"{0}\".\"{1}\"\n",frmM.m_strUserId.ToUpper(), stable2));
                sqry.Append("(\n");
                sqry.Append("\"NO\" NUMBER(8,0) NOT NULL ENABLE,\n");
                sqry.Append("\"BRGY\" VARCHAR2(50 BYTE) NOT NULL ENABLE,\n");
                sqry.Append("\"GDB\" NUMBER(8,0) NOT NULL ENABLE,\n");
                sqry.Append("\"RDB\" NUMBER(8,0) NOT NULL ENABLE,\n");
                sqry.Append("\"RDB_GDB\" NUMBER(8,0) NOT NULL ENABLE,\n");
                sqry.Append("\"GDB_RDB\" NUMBER(8,2) NOT NULL ENABLE,\n");
                sqry.Append("\"TPG\" NUMBER(8,0) NOT NULL ENABLE,\n");
                sqry.Append("\"TPG_GDB\" NUMBER(8,2) NOT NULL ENABLE,\n");
                sqry.Append("\"TPG_RDB\" NUMBER(8,2) NOT NULL ENABLE\n");
                sqry.Append(")");

                updateDBASE(frmM.m_strconn, sqry.ToString());
            }

            stable = "COORDINATES";
            sqry = new StringBuilder();
            sqry.Append(String.Format("select count(*) from user_tables where table_name= '{0}'", stable));
            strtblcnt = returnValue(sqry.ToString(), frmM.m_strconn, stable);
            if (strtblcnt == "" || strtblcnt == "0" || strtblcnt == null)
            {
                sqry = new StringBuilder();
                sqry.Append(String.Format("CREATE TABLE \"{0}\".\"{1}\"\n", frmM.m_strUserId.ToUpper(), stable));
                sqry.Append("(\n");
                sqry.Append("\"ID\"   NUMBER NOT NULL ENABLE,\n");
                sqry.Append("\"PROVINCE\"   VARCHAR2(100 BYTE),\n");
                sqry.Append("\"LGU\"   VARCHAR2(100 BYTE),\n");
                sqry.Append("\"COORDTYPE\"    VARCHAR2(100 BYTE),\n");
                sqry.Append("\"COORDNO\" NUMBER,\n");
                sqry.Append("\"NORTHINGS\" NUMBER,\n");
                sqry.Append("\"EASTINGS\" NUMBER\n");
                sqry.Append(")");
                updateDBASE(frmM.m_strconn, sqry.ToString());
            }

            stable = "COORDINATES_SQNC";
            sqry = new StringBuilder();
            sqry.Append(String.Format("select count(*) from user_sequences where sequence_name= '{0}'", stable));
            strtblcnt = returnValue(sqry.ToString(), frmM.m_strconn, stable);
            if (strtblcnt == "" || strtblcnt == "0" || strtblcnt == null)
            {
                sqry = new StringBuilder();
                sqry.Append(String.Format("CREATE SEQUENCE \"{0}\".\"{1}\" MINVALUE 1 MAXVALUE 999999999999999999999999999 INCREMENT BY 1 START WITH 1", frmM.m_strUserId.ToUpper(), stable));
                updateDBASE(frmM.m_strconn, sqry.ToString());
            }

            stable = "PARCELS";
            sqry = new StringBuilder();
            sqry.Append(String.Format("select count(*) from user_tables where table_name= '{0}'", stable));
            strtblcnt = returnValue(sqry.ToString(), frmM.m_strconn, stable);
            if (strtblcnt == "" || strtblcnt == "0" || strtblcnt == null)
            {
                sqry = new StringBuilder();
                sqry.Append(String.Format("CREATE TABLE \"{0}\".\"{1}\"\n", frmM.m_strUserId.ToUpper(), stable));
                sqry.Append("(\n");
                sqry.Append("\"ID\"   NUMBER(11,0) NOT NULL ENABLE,\n");
                sqry.Append("\"TCT_NO\"   VARCHAR2(250 BYTE),\n");
                sqry.Append("\"LOT_NO\"   VARCHAR2(250 BYTE),\n");
                sqry.Append("\"SURVEY_NO\"    VARCHAR2(250 BYTE),\n");
                sqry.Append("\"CLAIMANT\" VARCHAR2(250 BYTE),\n");
                sqry.Append("\"MONUMENT\" VARCHAR2(250 BYTE),\n");
                sqry.Append("\"ENCODED_DATE\" DATE,\n");
                sqry.Append("\"BARANGAY\" VARCHAR2(250 BYTE),\n");
                sqry.Append("\"ENCODER\"  VARCHAR2(250 BYTE)\n");
                sqry.Append(")");

                updateDBASE(frmM.m_strconn, sqry.ToString());
            }

            stable = "PARCEL_SQNC";
            sqry = new StringBuilder();
            sqry.Append(String.Format("select count(*) from user_sequences where sequence_name= '{0}'", stable));
            strtblcnt = returnValue(sqry.ToString(), frmM.m_strconn, stable);
            if (strtblcnt == "" || strtblcnt == "0" || strtblcnt == null)
            {
                sqry = new StringBuilder();
                sqry.Append(String.Format("CREATE SEQUENCE \"{0}\".\"{1}\" MINVALUE 1 MAXVALUE 999999999999999999999999999 INCREMENT BY 1 START WITH 1", frmM.m_strUserId.ToUpper(), stable));
                updateDBASE(frmM.m_strconn, sqry.ToString());
            }

            stable = "CORNERS";
            sqry = new StringBuilder();
            sqry.Append(String.Format("select count(*) from user_tables where table_name= '{0}'", stable));
            strtblcnt = returnValue(sqry.ToString(), frmM.m_strconn, stable);
            if (strtblcnt == "" || strtblcnt == "0" || strtblcnt == null)
            {
                sqry = new StringBuilder();
                sqry.Append(String.Format("CREATE TABLE \"{0}\".\"{1}\"\n", frmM.m_strUserId.ToUpper(), stable));
                sqry.Append("(\n");
                sqry.Append("\"ID\"   NUMBER(11,0) NOT NULL ENABLE,\n");
                sqry.Append("\"TCT_NO\"   VARCHAR2(250 BYTE),\n");
                sqry.Append("\"DISTANCE\"   NUMBER(11,2),\n");
                sqry.Append("\"NORTHINGS\"    VARCHAR2(250 BYTE),\n");
                sqry.Append("\"DEGREES\" NUMBER(11,0),\n");
                sqry.Append("\"MINUTES\" NUMBER(11,0),\n");
                sqry.Append("\"EASTINGS\" VARCHAR2(250 BYTE)\n");
                sqry.Append("\"CORNER_NO\" NUMBER(11,0)\n");
                sqry.Append(")");
                updateDBASE(frmM.m_strconn, sqry.ToString());
            }

            stable = "CORNER_SQNC";
            sqry = new StringBuilder();
            sqry.Append(String.Format("select count(*) from user_sequences where sequence_name= '{0}'", stable));
            string strtblcnt1 = returnValue(sqry.ToString(), frmM.m_strconn, stable);
            if (strtblcnt1 == "" || strtblcnt1 == "0" || strtblcnt1 == null)
            {
                sqry = new StringBuilder();
                sqry.Append(String.Format("CREATE SEQUENCE \"{0}\".\"{1}\" MINVALUE 1 MAXVALUE 999999999999999999999999999 INCREMENT BY 1 START WITH 1", frmM.m_strUserId.ToUpper(), stable));
                updateDBASE(frmM.m_strconn, sqry.ToString());
            }

            ////for revision feb 11 2014
            //check if the tmcr table exist
            //stable = "GIS_TMCR" + frmM.m_userid.ToUpper();
            stable = "GIS_TMCR";
            string strquery = String.Format("select count(*) from user_tables where table_name= '{0}'", stable);
            strtblcnt = returnValue(strquery, frmM.m_strconn, stable);
            if (strtblcnt == "" || strtblcnt == "0" || strtblcnt == null)
            {
                sqry = new StringBuilder();
                sqry.Append(String.Format("CREATE TABLE \"{0}\".\"{1}\"\n", frmM.m_strUserId.ToUpper(), stable));
                sqry.Append("(\n");
                sqry.Append("\"LAND_PIN\" VARCHAR2(23 BYTE),\n");
                sqry.Append("\"USERID\" VARCHAR2(100 BYTE)\n");
                sqry.Append(")");

                updateDBASE(frmM.m_strconn, sqry.ToString());
            }
        }

        public void gisdbasetablegrneeded()
        {
            //check for gr_database GIS_REVISION_VALUE
            string stable = "GIS_REVISION_VALUE";
            StringBuilder sqry = new StringBuilder();
            sqry.Append(String.Format("select count(*) from user_tables where table_name= '{0}'", stable));
            string strtblcnt = returnValue(sqry.ToString(), frmM.m_strconn, stable);
            if (strtblcnt == "" || strtblcnt == "0" || strtblcnt == null)
            {
                sqry = new StringBuilder();
                sqry.Append(String.Format("CREATE TABLE \"{0}\".\"{1}\"\n", frmM.m_strUserId.ToUpper(), stable));
                sqry.Append("(\n");
                sqry.Append("\"PIN\"   VARCHAR2(18 BYTE) NOT NULL ENABLE,\n");
                sqry.Append("\"UNIT_VALUE_GIS\"   NUMBER(11,0) NOT NULL ENABLE,\n");
                sqry.Append("\"UNIT_VALUE_RPTA\"   NUMBER(11,0) NOT NULL ENABLE,\n");
                sqry.Append("\"GIS_CODE\"    VARCHAR2(11 BYTE) NOT NULL ENABLE,\n");
                sqry.Append("\"BRGY\" VARCHAR2(3 BYTE) NOT NULL ENABLE,\n");
                sqry.Append("\"SECT\" VARCHAR2(3 BYTE) NOT NULL ENABLE,\n");
                sqry.Append("\"STREET\" VARCHAR2(50 BYTE) NOT NULL ENABLE,\n");
                sqry.Append("\"ZONE_CODE\" VARCHAR2(20 BYTE),\n");
                sqry.Append("\"ZONE_NM\" VARCHAR2(20 BYTE)\n");
                sqry.Append(")");
                updateDBASE(frmM.m_strconn, sqry.ToString());
            }


            //check for gr_database GIS_LAND_SKED
            stable = "GIS_LAND_SKED";
            sqry = new StringBuilder();
            sqry.Append(String.Format("select count(*) from user_tables where table_name= '{0}'", stable));
            strtblcnt = returnValue(sqry.ToString(), frmM.m_strconn, stable);
            if (strtblcnt == "" || strtblcnt == "0" || strtblcnt == null)
            {
                sqry = new StringBuilder();
                sqry.Append(String.Format("CREATE TABLE \"{0}\".\"{1}\"\n", frmM.m_strUserId.ToUpper(), stable));
                sqry.Append("(\n");
                sqry.Append("\"PIN\"   VARCHAR2(23 BYTE) NOT NULL ENABLE,\n");
                sqry.Append("\"CLASS\"   VARCHAR2(50 BYTE) NOT NULL ENABLE,\n");
                sqry.Append("\"SUBCLASS\"   VARCHAR2(5 BYTE) NOT NULL ENABLE,\n");
                sqry.Append("\"DIST_CODE\"    VARCHAR2(2 BYTE),\n");
                sqry.Append("\"BRGY_CODE\" VARCHAR2(3 BYTE),\n");
                sqry.Append("\"SECT_CODE\" VARCHAR2(3 BYTE),\n");
                sqry.Append("\"ZONE_CODE\" VARCHAR2(3 BYTE),\n");
                sqry.Append("\"LOCN_CODE\" VARCHAR2(3 BYTE),\n");
                sqry.Append("\"GR_CODE\" VARCHAR2(3 BYTE)  NOT NULL ENABLE,\n");
                sqry.Append("\"UVAL\" FLOAT(126),\n");
                sqry.Append("\"UPDATED_DT\" DATE,\n");
                sqry.Append("\"UPDATED_BY\" VARCHAR2(50 BYTE)\n");
                sqry.Append(")");
                updateDBASE(frmM.m_strconn, sqry.ToString());
            }

            //check for gr_database GIS_RPT_UNMATCH
            stable = "GIS_RPT_UNMATCH";
            sqry = new StringBuilder();
            sqry.Append(String.Format("select count(*) from user_tables where table_name= '{0}'", stable));
            strtblcnt = returnValue(sqry.ToString(), frmM.m_strconn, stable);
            if (strtblcnt == "" || strtblcnt == "0" || strtblcnt == null)
            {
                sqry = new StringBuilder();
                sqry.Append(String.Format("CREATE TABLE \"{0}\".\"{1}\"\n", frmM.m_strUserId.ToUpper(), stable));
                sqry.Append("(\n");
                sqry.Append("\"PIN\" VARCHAR2(18 BYTE) NOT NULL ENABLE\n");
                sqry.Append(")");

                updateDBASE(frmM.m_strconn, sqry.ToString());
            }

            //check for gr_database LAND_FILTER
            stable = "LAND_FILTER";
            sqry = new StringBuilder();
            sqry.Append(String.Format("select count(*) from user_tables where table_name= '{0}'", stable));
            strtblcnt = returnValue(sqry.ToString(), frmM.m_strconn, stable);
            if (strtblcnt == "" || strtblcnt == "0" || strtblcnt == null)
            {
                sqry = new StringBuilder();
                sqry.Append(String.Format("CREATE TABLE \"{0}\".\"{1}\"\n", frmM.m_strUserId.ToUpper(), stable));
                sqry.Append("(\n");
                sqry.Append("\"PIN\" VARCHAR2(23 BYTE)\n");
                sqry.Append(")");

                updateDBASE(frmM.m_strconn, sqry.ToString());
            }

        }

        public void gisdbasetablelandmarkcreate()
        {
            //string strtable = "GIS_LANDMARKS_FILTER_" + frmM.m_userid.ToUpper();//EDITED feb 11 2014
            string strtable = "GIS_LANDMARKS_FILTER";
            //check if the landmark filter table exist
            StringBuilder squery = new StringBuilder();
            squery.Append(String.Format("select count(*) from user_tables where table_name= '{0}'", strtable));
            string strtblcnt = returnValue(squery.ToString(), frmM.m_strconn, strtable);
            if (strtblcnt == "" || strtblcnt == "0" || strtblcnt == null)
            {
                squery = new StringBuilder();
                squery.Append(String.Format("CREATE TABLE \"{0}\".\"{1}\"\n", frmM.m_strUserId.ToUpper(), strtable));
                squery.Append("(\n");
                squery.Append("\"LM_CATEGORY\" VARCHAR2(100 BYTE),\n");
                squery.Append("\"USERID\" VARCHAR2(100 BYTE)\n");
                squery.Append(")");
                updateDBASE(frmM.m_strconn, squery.ToString());
            }

            //check if the landmarkcategory table exist
            strtable = "GIS_LANDMARKS";
            squery = new StringBuilder();
            squery.Append(String.Format("select count(*) from user_tables where table_name= '{0}'", strtable));
            String strtblcnt1 = returnValue(squery.ToString(), frmM.m_strconn, strtable);
            if (strtblcnt1 == "" || strtblcnt1 == "0" || strtblcnt1 == null)
            {
                squery = new StringBuilder();
                squery.Append(String.Format("CREATE TABLE \"{0}\".\"{1}\"\n", frmM.m_strUserId.ToUpper(), strtable));
                squery.Append("(\n");
                squery.Append("\"ID\" NUMBER NOT NULL ENABLE,\n");
                squery.Append("\"LM_NAME\"VARCHAR2(250 BYTE),\n");
                squery.Append("\"LM_DESC\"VARCHAR2(250 BYTE),\n");
                squery.Append("\"LM_TYPE\"VARCHAR2(250 BYTE),\n");
                squery.Append("\"LM_CATEGORY\" VARCHAR2(250 BYTE),\n");
                squery.Append("\"X\" NUMBER,\n");
                squery.Append("\"Y\" NUMBER\n");
                squery.Append(")");

                updateDBASE(frmM.m_strconn, squery.ToString());
            }

            //check if the landmarkcategory table exist
            string stable = "GIS_LM_SQNC";
            squery = new StringBuilder();
            squery.Append(String.Format("select count(*) from user_sequences where sequence_name= '{0}'", stable));
            strtblcnt = returnValue(squery.ToString(), frmM.m_strconn, stable);
            if (strtblcnt == "" || strtblcnt == "0" || strtblcnt == null)
            {
                squery = new StringBuilder();
                squery.Append(String.Format("CREATE SEQUENCE \"{0}\".\"{1}\" MINVALUE 1 MAXVALUE 999999999999999999999999999 INCREMENT BY 1 START WITH 1", frmM.m_strUserId.ToUpper(), stable));
                updateDBASE(frmM.m_strconn, squery.ToString());
            }

            //check if the landmarkcategory table exist
            stable = "GIS_LANDMARKCATEGORIES";
            squery = new StringBuilder();
            squery.Append(String.Format("select count(*) from user_tables where table_name= '{0}'", stable));
            strtblcnt = returnValue(squery.ToString(), frmM.m_strconn, stable);
            if (strtblcnt == "" || strtblcnt == "0" || strtblcnt == null)
            {
                squery = new StringBuilder();
                squery.Append(String.Format("CREATE TABLE \"{0}\".\"{1}\"\n", frmM.m_strUserId.ToUpper(), stable));
                squery.Append("(\n");
                squery.Append("\"ID\" NUMBER NOT NULL ENABLE,\n");
                squery.Append("\"LM_TYPE\"VARCHAR2(100 BYTE),\n");
                squery.Append("\"LM_CATEGORY\" VARCHAR2(100 BYTE),\n");
                squery.Append("\"LM_CATEGORY_CODE\" NUMBER\n");
                squery.Append(")");

                updateDBASE(frmM.m_strconn, squery.ToString());

                squery = new StringBuilder();
                squery.Append("Insert all\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (1,'BRGY. HALL','GOVERNMENT ENTITIES',1)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (4,'CHURCH','RELIGIOUS ENTITIES',2)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (5,'CINEMA','RECREATIONAL ENTITIES',7)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (6,'CONVINIENCE STORE','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (7,'COOPERATIVE','OTHERS',7)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (8,'DENTAL CLINIC','MEDICAL ENTITIES',4)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (9,'DEPARTMENT STORE','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (10,'DRUG STORE','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (11,'FACTORY','INDUSTRIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (12,'FIRE STATION','FIRE STATION',1)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (13,'FOOD CHAIN','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (14,'FUNERAL PARLOR','OTHERS',7)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (15,'GAS STATION','GAS STATION',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (16,'GROCERY','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (17,'HOSPITAL','MEDICAL ENTITIES',4)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (18,'INDUSTRY','INDUSTRIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (19,'MARKET','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (20,'MEDICAL CLINIC','MEDICAL ENTITIES',4)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (21,'MINI MART','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (22,'OFFICE','OTHERS',7)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (23,'ORPHANAGE','OTHERS',7)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (24,'POLICE OUTPOST','POLICE STATION',1)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (25,'POST','OTHERS',7)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (26,'RESORT','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (27,'RESTAURANT','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (29,'SHOPPING CENTER','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (31,'BANK','FINANCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (36,'CITY HALL','GOVERNMENT ENTITIES',1)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (33,'SCHOOL','EDUCATIONAL ENTITIES',3)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (34,'OTB','RECREATIONAL ENTITIES',7)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (35,'SUBDIVISION','SUBDIVISION',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (45,'BUS TERMINAL','TRANSPORTATION ENTITIES',5)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (37,'JEEPNEY TERMINAL','TRANSPORTATION ENTITIES',5)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (38,'FX TERMINAL','TRANSPORTATION ENTITIES',5)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (39,'TRICYCLE TERMINAL','TRANSPORTATION ENTITIES',5)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (40,'PEDICAB TERMINAL','TRANSPORTATION ENTITIES',5)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (41,'TAXI TERMINAL','TRANSPORTATION ENTITIES',5)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (42,'MRT STATION','TRANSPORTATION ENTITIES',5)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (43,'LRT STATION','TRANSPORTATION ENTITIES',5)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (44,'TRAIN STATION','TRANSPORTATION ENTITIES',5)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (46,'COVERED COURT','RECREATIONAL ENTITIES',7)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (47,'GYMNASIUM','RECREATIONAL ENTITIES',7)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (48,'OPEN PARKS','RECREATIONAL ENTITIES',7)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (49,'BETTING STATIONS','RECREATIONAL ENTITIES',7)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (50,'INTERNET CAFE','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (51,'LAUNDRY SHOP','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (52,'GOVERNMENT HOUSING','GOVERNMENT ENTITIES',1)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (53,'TENEMENT BUILDING','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (54,'APARTELLE','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (55,'HARDWARE','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (56,'VULCANIZING SHOP','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (57,'WATER STATION','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (58,'VETERINARY CLINIC','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (59,'PAWN SHOP','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (60,'OPTICAL CLINIC','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (61,'BAKERY','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (62,'PET SHOP','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (63,'CAR WASH','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (64,'SALON / SPA','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (65,'AUTO REPAIR SHOP','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (66,'BAYAD CENTER','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (67,'WAREHOUSE','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (68,'PLAZA','GOVERNMENT ENTITIES',1)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (69,'TELECOM','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (70,'AUDITORIUM','RECREATIONAL ENTITIES',7)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (71,'CEMETERY','GOVERNMENT ENTITIES',1)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (72,'SWIMMING POOL','RECREATIONAL ENTITIES',7)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (73,'PLAY GROUND','RECREATIONAL ENTITIES',7)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (74,'CLUB HOUSE','RECREATIONAL ENTITIES',7)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (75,'STUDIO','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (76,'LTO','GOVERNMENT ENTITIES',1)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (77,'CAR DEALER','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (78,'TENNIS COURT','RECREATIONAL ENTITIES',7)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (79,'TRANSMISSION TOWER','GOVERNMENT ENTITIES',1)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (80,'TRIAL COURT','GOVERNMENT ENTITIES',1)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (81,'MALL','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (82,'SOCIAL SECURITY SYSTEM','GOVERNMENT ENTITIES',1)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (83,'COURIER','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (84,'RICE MILL','COMMERCIAL ENTITIES',6)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (85,'SUBSTATION','GOVERNMENT ENTITIES',1)\n");
                squery.Append("into GIS_LANDMARKCATEGORIES (ID,LM_TYPE,LM_CATEGORY,LM_CATEGORY_CODE) values (86,'COCKPIT ARENA','COMMERCIAL ENTITIES',6)\n");
                squery.Append("select * from dual");
                updateDBASE(frmM.m_strconn, squery.ToString());
            }
        }

        public ArrayList populateDBASEBrgy(string strLayer, string strconn, string sqry)
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            ArrayList arrList = new ArrayList();
            try
            {
                OleDbConnection oledbconn = new OleDbConnection(strconn);
                OleDbCommand cmd = new OleDbCommand(sqry, oledbconn);
                OleDbDataAdapter newDataAdapter = new OleDbDataAdapter(cmd);
                DataSet newDataSet = new DataSet();
                oledbconn.Open();
                OleDbDataReader newDataReaders = cmd.ExecuteReader();

                if (newDataReaders.HasRows)
                {
                    while (newDataReaders.Read())
                    {
                        if (strLayer == "BARANGAY BOUNDARY")
                        {
                            if (!DBNull.Value.Equals(newDataReaders["BRGY_NM"]))
                                arrList.Add((String)newDataReaders["BRGY_NM"].ToString());
                        }
                        else if (strLayer == "SECTION BOUNDARY")
                        {
                            if (!DBNull.Value.Equals(newDataReaders["BRGY_NM"]))
                                arrList.Add((String)newDataReaders["BRGY_NM"].ToString());
                        }

                    }
                    newDataReaders.Close();
                    oledbconn.Close();
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

                }
                return arrList;
            }
            catch
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                return arrList;
            }
        }

        public void removeAllLayers()
        {
            try
            {
                for (int i = 0; i < frmM.legend1.Layers.Count; i++)
                {
                    Shapefile sf = (Shapefile)frmM.axMap.get_GetObject(i);
                    if (sf.Open(sf.Filename))
                    {
                        if (sf.EditingTable)
                            sf.StopEditingTable(false, null);
                        if (sf.EditingShapes)
                            sf.StopEditingShapes(false, false, null);

                        sf.Categories = null;
                        sf.Labels = null;
                        sf.Table.Close();
                        sf.Close();
                        
                    }
                }
                if (frmM.legend1.Layers.Count > 0)
                    frmM.legend1.Layers.Clear();
                if(frmM.legend1.Groups.Count > 0)
                    frmM.legend1.Groups.Clear();

                frmM.legend1.Invalidate();
                frmM.legend1.Refresh();
            }
            catch
            {
                if (frmM.legend1.Layers.Count > 0)
                    frmM.legend1.Layers.Clear();
                if (frmM.legend1.Groups.Count > 0)
                    frmM.legend1.Groups.Clear();
                //MessageBox.Show(err.Message,"Removing Layers");
            }

        }

        public void clearMapSelect()
        {
            Shapefile sf = new Shapefile();
            for (int i = 0; i < frmM.legend1.Layers.Count; i++)
            {
                if (frmM.legend1.Map.get_LayerName(i).ToUpper() == "BARANGAY BOUNDARY")
                {
                    sf = (Shapefile)frmM.axMap.get_GetObject(i);
                    sf.SelectNone();
                }
                if (frmM.legend1.Map.get_LayerName(i).ToUpper() == "SECTION BOUNDARY")
                {
                    sf = (Shapefile)frmM.axMap.get_GetObject(i);
                    sf.SelectNone();
                }
            }
            frmM.axMap.ClearDrawings();
            frmM.legend1.Refresh();
            frmM.legend1.Invalidate();
            frmM.legend1.Map.Redraw();
        }

        public bool IsFileUsedbyAnotherProcess(string filename)
        {
            try
            {
                File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (System.IO.IOException exp)
            {
                return true;
            }
            return false;

        }

        private void MarkAllVertices(double curX, double curY)
        {
            try
            {
                int handle;

               // if (m_globals.CurrentLayer == null) return;
                //handle = m_MapWin.Layers.CurrentLayer;
                //MapWinGIS.Shapefile shpFile = m_globals.CurrentLayer;
                //int numShp = shpFile.NumShapes;
                //int shpIndex;

                //if (m_prevShape != -1)
                //{
                //    if (!m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].VerticesVisible)
                //        m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].HideVertices();
                //}

                //if (m_MapWin.Layers[m_MapWin.Layers.CurrentLayer].LayerType == MapWindow.Interfaces.eLayerType.PolygonShapefile)
                //{
                //    if (shpFile.BeginPointInShapefile())
                //    {
                //        shpIndex = shpFile.PointInShapefile(curX, curY);
                //        shpFile.EndPointInShapefile();
                //    }
                //    else
                //        shpIndex = -1;
                //}
                //else
                //{
                //    MapWinGIS.Extents bounds = new MapWinGIS.ExtentsClass();
                //    bounds.SetBounds(curX, curY, 0, curX, curY, 0);
                //    object resArray = null;
                //    if (shpFile.SelectShapes(bounds, m_globals.CurrentTolerance * 2, MapWinGIS.SelectMode.INTERSECTION, ref resArray))
                //    {
                //        shpIndex = (int)((System.Array)resArray).GetValue(0);
                //    }
                //    else
                //        shpIndex = -1;
                //}

                //if (shpIndex >= 0)
                //{
                //    m_MapWin.Layers[handle].Shapes[shpIndex].ShowVertices(System.Drawing.Color.Red, m_globals.VertexSize);
                //    m_prevShape = shpIndex;
                //}
                //else
                //    m_prevShape = -1;
            }
            catch (System.Exception ex)
            {
                //m_MapWin.ShowErrorDialog(ex);
            }
        }

        #region private SectionPolygonWithLine()
        //Angela Hillier 10/05
        /// <summary>
        /// Sections a polygon into multiple parts depending on where line crosses it and if previous sectioning has occured.
        /// </summary>
        /// <param name="line">The line that splits the polygon. First and last points are intersect points.</param>
        /// <param name="polygon">The polygon that is to be split by the line.</param>
        /// <param name="polyStart">Index to polygon segment where the first intersect point is found.</param>
        /// <param name="polyEnd">Index to polygon segment where last intersect point is found.</param>
        /// <param name="resultSF">Reference to result shapefile where new polygon sections will be saved.</param>
        /// <returns>False if an error occurs, true otherwise.</returns>
        private static bool SectionPolygonWithLine(ref MapWinGIS.Shape line, ref MapWinGIS.Shape polygon, int polyStart, int polyEnd, ref MapWinGIS.Shapefile resultSF)
        {
            int shpIndex = 0;
            int numResults = resultSF.NumShapes;
            bool previousSplits = false;
            if (numResults != 0)
            {
                previousSplits = true;
            }

            //we can now make two new polygons by splitting the original one with the line segment
            MapWinGIS.Shape poly1 = new MapWinGIS.ShapeClass();
            MapWinGIS.Shape poly2 = new MapWinGIS.ShapeClass();

            SplitPolyInTwo(ref line, ref polygon, polyStart, polyEnd, out poly1, out poly2);

            if (previousSplits == false)
            {
                shpIndex = 0;
                resultSF.EditInsertShape(poly1, ref shpIndex);
                shpIndex = 1;
                resultSF.EditInsertShape(poly2, ref shpIndex);
            }
            //else
            //{
            //    //this polygon underwent previous splittings, check
            //    //if the new results overlay the old ones before adding to resultSF
            //    string tempPath = System.IO.Path.GetTempPath();
            //    string tempFile1 = tempPath + "test1SF.shp";
            //    DataManagement.DeleteShapefile(ref tempFile1);
            //    MapWinGIS.ShpfileType sfType = resultSF.ShapefileType;
            //    MapWinGIS.Shapefile test1SF = new MapWinGIS.ShapefileClass();
            //    //CDM 8/4/2006 test1SF.CreateNew(tempFile1, sfType);
            //    Globals.PrepareResultSF(ref tempFile1, ref test1SF, sfType);

            //    string tempFile2 = tempPath + "test2SF.shp";
            //    DataManagement.DeleteShapefile(ref tempFile2);
            //    MapWinGIS.Shapefile test2SF = new MapWinGIS.ShapefileClass();
            //    //CDM 8/4/2006 test2SF.CreateNew(tempFile2, sfType);
            //    Globals.PrepareResultSF(ref tempFile2, ref test2SF, sfType);

            //    if (ClipPolySFWithPoly.ClipPolygonSFWithPolygon(ref resultSF, ref poly1, out test1SF, false) == false)
            //    {
            //        gErrorMsg = "Problem clipping polygon: " + Error.GetLastErrorMsg();
            //        Error.SetErrorMsg(gErrorMsg);
            //        Debug.WriteLine(gErrorMsg);
            //        return false;
            //    }
            //    if (ClipPolySFWithPoly.ClipPolygonSFWithPolygon(ref resultSF, ref poly2, out test2SF, false) == false)
            //    {
            //        gErrorMsg = "Problem clipping polygon: " + Error.GetLastErrorMsg();
            //        Error.SetErrorMsg(gErrorMsg);
            //        Debug.WriteLine(gErrorMsg);
            //        return false;
            //    }

            //    if (test1SF.NumShapes > 0 || test2SF.NumShapes > 0)
            //    {
            //        for (int j = numResults - 1; j >= 0; j--)
            //        {
            //            if (resultSF.EditDeleteShape(j) == false)
            //            {
            //                gErrorMsg = "Problem deleting intermediate polygon: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
            //                Error.SetErrorMsg(gErrorMsg);
            //                Debug.WriteLine(gErrorMsg);
            //                return false;
            //            }
            //        }
            //        int numTestShapes = test1SF.NumShapes;
            //        int insertIndex = 0;
            //        MapWinGIS.Shape insertShape = new MapWinGIS.ShapeClass();
            //        for (int j = 0; j <= numTestShapes - 1; j++)
            //        {
            //            insertShape = test1SF.get_Shape(j);
            //            if (insertShape.numPoints > 0)
            //            {
            //                if (resultSF.EditInsertShape(insertShape, ref insertIndex) == false)
            //                {
            //                    gErrorMsg = "Problem inserting polygon into result file: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
            //                    Error.SetErrorMsg(gErrorMsg);
            //                    Debug.WriteLine(gErrorMsg);
            //                    return false;
            //                }
            //            }
            //        }
            //        numTestShapes = test2SF.NumShapes;
            //        for (int j = 0; j <= numTestShapes - 1; j++)
            //        {
            //            insertShape = test2SF.get_Shape(j);
            //            if (insertShape.numPoints > 0)
            //            {
            //                if (resultSF.EditInsertShape(insertShape, ref insertIndex) == false)
            //                {
            //                    gErrorMsg = "Problem inserting polygon into result file: " + resultSF.get_ErrorMsg(resultSF.LastErrorCode);
            //                    Error.SetErrorMsg(gErrorMsg);
            //                    Debug.WriteLine(gErrorMsg);
            //                    return false;
            //                }
            //            }
            //        }
            //    }
            //}//end of checking against previous splits
            return true;
        }
        #endregion

        #region private SplitPolyInTwo() -- used by SectionPolygonWithLine()
        //Angela Hillier 10/05
        /// <summary>
        /// Splits original polygon into two portions depending on where line crosses it.
        /// </summary>
        /// <param name="line">The line the crosses the polygon. First and last points are intersects.</param>
        /// <param name="polygon">The polygon that is split by the line.</param>
        /// <param name="beginPolySeg">The section of the polygon where the first intersect point is found.</param>
        /// <param name="endPolySeg">The section of the polygon where the last intersect point is found.</param>
        /// <param name="poly1">First portion of polygon returned after splitting.</param>
        /// <param name="poly2">Second portion of polygon returned after splitting.</param>
        private static void SplitPolyInTwo(ref MapWinGIS.Shape line, ref MapWinGIS.Shape polygon, int beginPolySeg, int endPolySeg, out MapWinGIS.Shape poly1, out MapWinGIS.Shape poly2)
        {
            //function assumes first and last pts in line are the two intersection pts
            MapWinGIS.Shape firstPart = new MapWinGIS.ShapeClass();
            MapWinGIS.Shape secondPart = new MapWinGIS.ShapeClass();
            MapWinGIS.Utils utils = new MapWinGIS.UtilsClass();
            MapWinGIS.ShpfileType shpType = polygon.ShapeType;
            firstPart.ShapeType = shpType;
            secondPart.ShapeType = shpType;
            int numPolyPts = polygon.numPoints;
            int numLinePts = line.numPoints;
            int ptIndex = 0;
            bool crossZeroPt = false;
            int count = 0;

            //now, see if we'll be crossing the zero pt while building the first result poly
            if (beginPolySeg < endPolySeg + 1)
            {
                crossZeroPt = true;
            }
            else
            {
                crossZeroPt = false;
            }

            //split the poly into two portions
            //begin by creating the side where the line will be inserted in the forward direction
            //add all line pts in forward direction
            for (int i = 0; i <= numLinePts - 1; i++)
            {
                ptIndex = firstPart.numPoints;
                firstPart.InsertPoint(line.get_Point(i), ref ptIndex);
            }
            //add polygon pts that are clockwise of the ending line point
            if (crossZeroPt == true)
            {
                //we'll be crossing the zero point when creating a clockwise poly
                int position;
                count = (numPolyPts - 1) - (endPolySeg + 1);
                //add all points before the zero point and clockwise of last point in line
                for (int i = 0; i <= count - 1; i++)
                {
                    position = (endPolySeg + 1) + i;
                    ptIndex = firstPart.numPoints;
                    firstPart.InsertPoint(polygon.get_Point(position), ref ptIndex);
                }
                //add all points after the zero point and up to first line point
                for (int i = 0; i <= beginPolySeg; i++)
                {
                    ptIndex = firstPart.numPoints;
                    firstPart.InsertPoint(polygon.get_Point(i), ref ptIndex);
                }
            }
            else
            {
                //we don't need to worry about crossing the zero point
                for (int i = endPolySeg + 1; i <= beginPolySeg; i++)
                {
                    ptIndex = firstPart.numPoints;
                    firstPart.InsertPoint(polygon.get_Point(i), ref ptIndex);
                }
            }
            //add beginning line point to close the new polygon
            ptIndex = firstPart.numPoints;
            firstPart.InsertPoint(line.get_Point(0), ref ptIndex);

            //create second portion by removing first from original polygon
            //secondPart = utils.ClipPolygon(MapWinGIS.PolygonOperation.DIFFERENCE_OPERATION, polygon, firstPart);
            //above method (difference) adds unnecessary points to the resulting shape, use below instead.
            //begin by creating the side where the line will be inserted in the forward direction
            //add line pts in reverse order
            for (int i = numLinePts - 1; i >= 0; i--)
            {
                ptIndex = secondPart.numPoints;
                secondPart.InsertPoint(line.get_Point(i), ref ptIndex);
            }
            //add polygon pts that are clockwise of the first line point
            //This may be confusing, but if crossZeroPt was true above, then it would
            //mean that the secondPart does not require crossing over the zero pt.
            //However, if crossZeroPt was false before, then secondPart will require
            //crossing the zeroPt while adding the polygon pts to the new shape.
            if (crossZeroPt == false)
            {
                //we'll be crossing the zero point when creating the second poly
                int position;
                count = (numPolyPts - 1) - (beginPolySeg + 1);
                //add all points before the zero point and clockwise of first point in line
                for (int i = 0; i <= count - 1; i++)
                {
                    position = (beginPolySeg + 1) + i;
                    ptIndex = secondPart.numPoints;
                    secondPart.InsertPoint(polygon.get_Point(position), ref ptIndex);
                }
                //add all points after the zero point and up to last line point
                for (int i = 0; i <= endPolySeg; i++)
                {
                    ptIndex = secondPart.numPoints;
                    secondPart.InsertPoint(polygon.get_Point(i), ref ptIndex);
                }
            }
            else
            {
                //we don't need to worry about crossing the zero point
                for (int i = beginPolySeg + 1; i <= endPolySeg; i++)
                {
                    ptIndex = secondPart.numPoints;
                    secondPart.InsertPoint(polygon.get_Point(i), ref ptIndex);
                }
            }
            //add ending line point to close the new polygon
            ptIndex = secondPart.numPoints;
            secondPart.InsertPoint(line.get_Point(numLinePts - 1), ref ptIndex);

            //return the two polygon portions
            poly1 = firstPart;
            poly2 = secondPart;

            //output results to screen for testing
            //Debug.WriteLine("poly1 numPoints = " + firstPart.numPoints);
            //			string poly1Points = "";
            //			for(int i = 0; i <= firstPart.numPoints-1; i++)
            //			{
            //				poly1Points += "(" + firstPart.get_Point(i).x + ", " + firstPart.get_Point(i).y + "), ";
            //			}
            //Debug.WriteLine(poly1Points);
            //Debug.WriteLine("poly2 numPoints = " + secondPart.numPoints);
            //			string poly2Points = "";
            //			for(int i = 0; i <= secondPart.numPoints-1; i++)
            //			{
            //				poly2Points += "(" + secondPart.get_Point(i).x + ", " + secondPart.get_Point(i).y + "), ";
            //			}
            //Debug.WriteLine(poly2Points);		

        }
        #endregion

        #region private ProcessAllOutside() -- used by Accurate_ClipPolygonWithLine()
        //Angela Hillier 10/05
        /// <summary>
        /// For lines where every point lies outside the polygon, this function will
        /// find if any 2pt segment crosses through the polygon. If so, it will split
        /// the polygon into mutliple parts using the intersecting line segments.
        /// </summary>
        /// <param name="line">The line whose points are all inside the polygon.</param>
        /// <param name="polygon">The polygon being checked for intersection.</param>
        /// <param name="resultSF">The file where new polygon sections should be saved to.</param>
        /// <returns>False if errors were encountered, true otherwise.</returns>
        private static bool ProcessAllOutside(ref MapWinGIS.Shape line, ref MapWinGIS.Shape polygon, ref MapWinGIS.Shapefile resultSF)
        {
            int numLinePts = line.numPoints;
            int numLineSegs = numLinePts - 1;
            int numPolyPts = polygon.numPoints;
            int[] intersectsPerSeg = new int[numLineSegs];
            MapWinGIS.Point[][] intersectPts = new MapWinGIS.Point[numLineSegs][];
            int[][] polyIntLocs = new int[numLineSegs][]; //intersection occurs between polygon point indexed by polyIntLoc[][] and the previous point.

            //cut line into 2pt segments and put in new shapefile.
            string tempPath = System.IO.Path.GetTempPath() + "tempLineSF.shp";
            MapWinGeoProc.DataManagement.DeleteShapefile(ref tempPath);
            MapWinGIS.Shapefile lineSegSF = new MapWinGIS.ShapefileClass();
            //CDM 8/4/2006 lineSegSF.CreateNew(tempPath, line.ShapeType);
            MapWinGeoProc.Globals.PrepareResultSF(ref tempPath, ref lineSegSF, line.ShapeType);
            int shpIndex = 0;
            MapWinGIS.Shape lineSegment;
            for (int i = 0; i <= numLineSegs - 1; i++)
            {
                lineSegment = new MapWinGIS.ShapeClass();
                lineSegment.ShapeType = line.ShapeType;
                int ptIndex = 0;
                lineSegment.InsertPoint(line.get_Point(i), ref ptIndex);
                ptIndex = 1;
                lineSegment.InsertPoint(line.get_Point(i + 1), ref ptIndex);
                shpIndex = lineSegSF.NumShapes;
                lineSegSF.EditInsertShape(lineSegment, ref shpIndex);

                intersectPts[i] = new MapWinGIS.Point[numPolyPts];
                polyIntLocs[i] = new int[numPolyPts];
            }

            int numIntersects = 4;//CalcSiDeterm(ref lineSegSF as MapWindow.Data.IFeatureSet, ref polygon as MapWindow.Data.IFeature, out intersectsPerSeg, out intersectPts, out polyIntLocs);

            if (numIntersects == 0)
            {
                //entire line is outside the polygon, no splitting occurs
            }
            else
            {
                //intersections exist! Find out where.
                MapWinGIS.Shape intersectSeg = new MapWinGIS.ShapeClass();
                intersectSeg.ShapeType = line.ShapeType;
                int ptIndex = 0;

                for (int i = 0; i <= numLineSegs - 1; i++)
                {
                    int numSegIntersects = intersectsPerSeg[i];
                    //if there are less than 2 intersects, the line will not cross the 
                    //polygon in such a way that a new polygon section can be created.
                    if (numSegIntersects == 0)
                    {
                        //outside lines should be ignored, we only want a portions that cross
                        //the polygon.
                        int c = i + 1;
                        while (intersectsPerSeg[c] == 0 && c <= numLineSegs - 1)
                        {
                            c++;
                            if (c == numLineSegs)
                            {
                                break;
                            }
                        }
                        i = c - 1;
                    }
                    else
                    {
                        //there should always be an even # of intersects for a line of all outside pts
                        //find the intersecting segments that will split the polygon
                        MapWinGIS.Point[] intPts = new MapWinGIS.Point[numSegIntersects];
                        MapWinGIS.Point startPt = new MapWinGIS.PointClass();
                        startPt = lineSegSF.get_Shape(i).get_Point(0);

                        MapWinGeoProc.Globals.FindAndSortValidIntersects(numSegIntersects, ref intersectPts[i], ref intPts, ref startPt, ref polyIntLocs[i]);

                        for (int j = 0; j <= numSegIntersects - 1; j++)
                        {
                            ptIndex = 0;
                            intersectSeg.InsertPoint(intPts[j], ref ptIndex);
                            ptIndex = 1;
                            intersectSeg.InsertPoint(intPts[j + 1], ref ptIndex);
                            int polyStartIndex = polyIntLocs[i][j] - 1;
                            int polyEndIndex = polyIntLocs[i][j + 1] - 1;
                            if (SectionPolygonWithLine(ref intersectSeg, ref polygon, polyStartIndex, polyEndIndex, ref resultSF) == false)
                            {
                                //gErrorMsg = "Problem sectioning polygon: " + Error.GetLastErrorMsg();
                                //Error.SetErrorMsg(gErrorMsg);
                                Debug.WriteLine(gErrorMsg);
                                return false;
                            }
                            intersectSeg = new MapWinGIS.ShapeClass();
                            intersectSeg.ShapeType = line.ShapeType;
                            j++;
                        }//end of looping through intersect pts
                    }//end of else intersects exist for 2pt segment	
                }//end of looping through 2pt line segments
            }//end of else intersects exist
            return true;
        }
        #endregion

        private static int CalcSiDeterm(ref MapWindow.Data.IFeatureSet lineSF, ref MapWindow.Data.IFeature polygon, ref int[] intersectsPerLineSeg, ref MapWindow.Geometries.Point[][] intersectionPts, ref int[][] polyIntersectLocs)
        //private static int CalcSiDeterm(ref MapWinGIS.Shapefile lineSF, ref MapWinGIS.Shape polygon, ref int[] intersectsPerLineSeg, ref MapWindow.Geometries.Point[][] intersectionPts, ref int[][] polyIntersectLocs)
        {
            int numSignChanges = 0; //tracks number of determinant sign changes
            //int numLines = lineSF.NumShapes;//lineSF.Features.Count;
            int numLines = lineSF.Features.Count;
            //int numVerticies = polygon.numPoints;//polygon.NumPoints;
            int numVerticies = polygon.NumPoints;
            int[][] detSigns = new int[numLines][];
            bool[][] signChanges = new bool[numLines][]; //keeps track of where sign changes occur
            int[][] changeLocations = new int[numLines][];
            int[] intersectsPerLine = new int[numLines];
            MapWindow.Geometries.Point[][] intersectPts = new MapWindow.Geometries.Point[numLines][];


            IList<MapWindow.Geometries.Coordinate> coorPoly = polygon.Coordinates;
            //ICoordinate[] secCoor = new ICoordinate[2];
            for (int lineNo = 0; lineNo <= numLines - 1; lineNo++)
            {
                //MapWinGIS.Shapefile sf = new Shapefile();
                //sf.NumShapes
                //MapWinGIS.Shape shp = new MapWinGIS.Shape();
                //MapWindow.Geometries.IBasicGeometry bg = shp as MapWindow.Geometries.IBasicGeometry;

                //MapWindow.Data.IFeature line = lineSF.get_Shape(lineNo) as MapWindow.Data.IFeature;//.Features[lineNo];
                MapWindow.Data.IFeature line = lineSF.Features[lineNo];
                IList<MapWindow.Geometries.Coordinate> secCoor;
                IList<MapWindow.Geometries.Coordinate> coorLine = line.Coordinates;
                int numChangesPerLine = 0;
                detSigns[lineNo] = new int[numVerticies];
                signChanges[lineNo] = new bool[numVerticies];
                intersectPts[lineNo] = new MapWindow.Geometries.Point[numVerticies];
                changeLocations[lineNo] = new int[numVerticies];

                for (int vertNo = 0; vertNo <= numVerticies - 1; vertNo++)
                {
                    intersectPts[lineNo][vertNo] = new MapWindow.Geometries.Point();
                    MapWindow.Geometries.Point intersectPt = new MapWindow.Geometries.Point();
                    // Calculate the determinant (3x3 square matrix)
                    double si = TurboDeterm(coorPoly[vertNo].X, coorLine[0].X, coorLine[1].X,
                        coorPoly[vertNo].Y, coorLine[0].Y, coorLine[1].Y);

                    // Check the determinant result
                    switch (vertNo)
                    {
                        case 0:
                            if (si == 0)
                                detSigns[lineNo][vertNo] = 0; // we have hit a vertex
                            else if (si > 0)
                                detSigns[lineNo][vertNo] = 1; // +'ve
                            else if (si < 0)
                                detSigns[lineNo][vertNo] = -1; // -'ve
                            signChanges[lineNo][0] = false;		// First element will NEVER be a sign change
                            break;
                        default:
                            if (si == 0)
                                detSigns[lineNo][vertNo] = 0;
                            else if (si > 0)
                                detSigns[lineNo][vertNo] = 1;
                            else if (si < 0)
                                detSigns[lineNo][vertNo] = -1;

                            // Check for sign change
                            if (detSigns[lineNo][vertNo - 1] != detSigns[lineNo][vertNo])
                            {
                                secCoor = new List<MapWindow.Geometries.Coordinate>();
                                secCoor.Add(coorPoly[vertNo - 1]);
                                secCoor.Add(coorPoly[vertNo]);
                                //calculate the actual intercept point	
                                MapWindow.Geometries.LineString polyTestLine1 = new MapWindow.Geometries.LineString(secCoor);
                                secCoor = new List<MapWindow.Geometries.Coordinate>();
                                secCoor.Add(coorLine[0]);
                                secCoor.Add(coorLine[1]);
                                MapWindow.Geometries.LineString polyTestLine2 = new MapWindow.Geometries.LineString(secCoor);
                                bool validIntersect = polyTestLine1.Intersects(polyTestLine2);
                                MapWindow.Geometries.IGeometry inPt = polyTestLine1.Intersection(polyTestLine2);
                                if (inPt.Coordinates.Count == 1)
                                    intersectPt = new MapWindow.Geometries.Point(inPt.Coordinate);

                                if (validIntersect)
                                {
                                    signChanges[lineNo][vertNo] = true;
                                    numSignChanges += 1;
                                    numChangesPerLine += 1;
                                    intersectsPerLine[lineNo] = numChangesPerLine;
                                    //we want to store the valid intersect pts at the
                                    //beginning of the array so we don't have to search for them
                                    intersectPts[lineNo][numChangesPerLine - 1] = intersectPt;
                                    //keep track of where the intersect occurs in reference to polygon
                                    changeLocations[lineNo][numChangesPerLine - 1] = vertNo; //intersect pt occurs between vertNo-1 and vertNo
                                }
                                else
                                {
                                    signChanges[lineNo][vertNo] = false;
                                }

                            }
                            else
                            {
                                signChanges[lineNo][vertNo] = false;
                            }
                            break;
                    }//end of switch

                }
            }
            polyIntersectLocs = changeLocations;
            intersectionPts = intersectPts;
            intersectsPerLineSeg = intersectsPerLine;
            return numSignChanges;
        }

        #region private ProcessAllInside() -- used by Accurate_ClipPolygonWithLine()
        //Angela Hillier 10/05
        /// <summary>
        /// For lines where every point lies within the polygon, this function will
        /// find if any 2pt segment crosses through the polygon. If so, it will split
        /// the polygon into mutliple parts using the intersecting line segments.
        /// </summary>
        /// <param name="line">The line whose points are all inside the polygon.</param>
        /// <param name="polygon">The polygon being checked for intersection.</param>
        /// <param name="resultSF">The file where new polygon sections should be saved to.</param>
        /// <returns>False if errors were encountered, true otherwise.</returns>
        private static bool ProcessAllInside(ref MapWinGIS.Shape line, ref MapWinGIS.Shape polygon, ref MapWinGIS.Shapefile resultSF)
        {
            int numLinePts = line.numPoints;
            int numLineSegs = numLinePts - 1;
            int numPolyPts = polygon.numPoints;
            int[] intersectsPerSeg = new int[numLineSegs];
            MapWinGIS.Point[][] intersectPts = new MapWinGIS.Point[numLineSegs][];
            int[][] polyIntLocs = new int[numLineSegs][]; //intersection occurs between polygon point indexed by polyIntLoc[][] and the previous point.

            //cut line into 2pt segments and put in new shapefile.
            MapWinGIS.Shapefile lineSegSF = new MapWinGIS.ShapefileClass();
            string tempPath = System.IO.Path.GetTempPath() + "tempLineSF.shp";
            MapWinGeoProc.DataManagement.DeleteShapefile(ref tempPath);
            //CDM 8/4/2006 lineSegSF.CreateNew(tempPath, line.ShapeType);
            MapWinGeoProc.Globals.PrepareResultSF(ref tempPath, ref lineSegSF, line.ShapeType);
            int shpIndex = 0;
            MapWinGIS.Shape lineSegment;
            for (int i = 0; i <= numLineSegs - 1; i++)
            {
                lineSegment = new MapWinGIS.ShapeClass();
                lineSegment.ShapeType = line.ShapeType;
                int ptIndex = 0;
                lineSegment.InsertPoint(line.get_Point(i), ref ptIndex);
                ptIndex = 1;
                lineSegment.InsertPoint(line.get_Point(i + 1), ref ptIndex);
                shpIndex = lineSegSF.NumShapes;
                lineSegSF.EditInsertShape(lineSegment, ref shpIndex);

                intersectPts[i] = new MapWinGIS.Point[numPolyPts];
                polyIntLocs[i] = new int[numPolyPts];
            }

            //MapWinGeoProc.Globals
            int numIntersects = 4;//CalcSiDeterm(ref lineSegSF as MapWindow.Data.IFeatureSet, ref polygon as MapWindow.Data.IFeature, out intersectsPerSeg, out intersectPts, out polyIntLocs);

            if (numIntersects == 0)
            {
                //entire line is inside the polygon, no splitting occurs
            }
            else
            {
                //intersections exist! Find out where.
                MapWinGIS.Shape intersectSeg = new MapWinGIS.ShapeClass();
                intersectSeg.ShapeType = line.ShapeType;
                int ptIndex = 0;
                for (int i = 0; i <= numLineSegs - 1; i++)
                {
                    int numSegIntersects = intersectsPerSeg[i];
                    //if there are less than 4 intersects, the line will not cross the 
                    //polygon in such a way that a new polygon section can be created.
                    if (numSegIntersects <= 2)
                    {
                        //inside lines should be ignored, we only want a portion that crosses
                        //the polygon.
                        int c = i + 1;
                        while (intersectsPerSeg[c] <= 2 && c <= numLineSegs - 1)
                        {
                            c++;
                            if (c == numLineSegs)
                            {
                                break;
                            }
                        }
                        i = c - 1;
                    }
                    else
                    {	//there should always be an even # of intersects for a line of all inside pts
                        //find intersecting segments that will split the polygon
                        MapWinGIS.Point[] intPts = new MapWinGIS.Point[numSegIntersects];
                        MapWinGIS.Point startPt = new MapWinGIS.PointClass();
                        startPt = lineSegSF.get_Shape(i).get_Point(0);

                        MapWinGeoProc.Globals.FindAndSortValidIntersects(numSegIntersects, ref intersectPts[i], ref intPts, ref startPt, ref polyIntLocs[i]);

                        for (int j = 0; j <= numSegIntersects - 1; j++)
                        {
                            if (j == 0 || j == numSegIntersects - 1)
                            {
                                //Any segment formed from inside pt -> intersect pt
                                //or intersect pt -> inside pt will NOT cross the polygon.
                            }
                            else
                            {
                                ptIndex = 0;
                                intersectSeg.InsertPoint(intPts[j], ref ptIndex);
                                ptIndex = 1;
                                intersectSeg.InsertPoint(intPts[j + 1], ref ptIndex);
                                int polyStartIndex = polyIntLocs[i][j] - 1;
                                int polyEndIndex = polyIntLocs[i][j + 1] - 1;
                                if (SectionPolygonWithLine(ref intersectSeg, ref polygon, polyStartIndex, polyEndIndex, ref resultSF) == false)
                                {
                                   // gErrorMsg = "Problem sectioning polygon: " + Error.GetLastErrorMsg();
                                    //Error.SetErrorMsg(gErrorMsg);
                                    Debug.WriteLine(gErrorMsg);
                                    return false;
                                }
                                intersectSeg = new MapWinGIS.ShapeClass();
                                intersectSeg.ShapeType = line.ShapeType;
                                j++;
                            }
                        }//end of looping through intersect pts
                    }//end of more than 2 intersects exist		

                }//end of looping through 2pt line segments
            }//end of else intersects exist
            return true;
        }
        #endregion

        #region Accurate_ClipPolygonWithLine()

        //#region in-memory version
        public static bool Accurate_ClipPolygonWithLine(ref MapWinGIS.Shape polygon, ref MapWinGIS.Shape line, out MapWinGIS.Shapefile resultSF)
        {
            MapWinGeoProc.Error.ClearErrorLog();
            MapWinGIS.Shapefile resultFile = new MapWinGIS.ShapefileClass();
            string resultFilePath = System.IO.Path.GetTempPath() + "tempResultSF.shp";

            if (polygon != null && line != null)
            {
                MapWinGIS.ShpfileType sfType = new MapWinGIS.ShpfileType();
                sfType = polygon.ShapeType;
                //make sure we are dealing with a valid shapefile type
                if (sfType == MapWinGIS.ShpfileType.SHP_POLYGON || sfType == MapWinGIS.ShpfileType.SHP_POLYGONM || sfType == MapWinGIS.ShpfileType.SHP_POLYGONZ)
                {
                    //create the result shapefile if it does not already exist
                    if (MapWinGeoProc.Globals.PrepareResultSF(ref resultFilePath, ref resultFile, sfType) == false)
                    {
                        resultSF = resultFile;
                        return false;
                    }

                    bool boundsIntersect = MapWinGeoProc.Globals.CheckBounds(ref line, ref polygon);

                    if (boundsIntersect == false)
                    {
                        gErrorMsg = "Line does not cross polygon boundary.";
                        Debug.WriteLine(gErrorMsg);
                        MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                        resultSF = resultFile;
                        return false;
                    }
                    else
                    {
                        //find if all of the line is inside, outside, or part in and out of polygon
                        //line might intersect polygon mutliple times
                        int numPoints = line.numPoints;
                        bool[] ptsInside = new bool[numPoints];
                        //MapWinGIS.Utils utils = new MapWinGIS.UtilsClass();
                        MapWinGIS.Point currPt = new MapWinGIS.PointClass();
                        MapWinGIS.Point nextPt = new MapWinGIS.PointClass();
                        int numInside = 0;
                        int numOutside = 0;

                        int numParts = polygon.NumParts;
                        if (numParts == 0)
                        {
                            numParts = 1;
                        }
                        MapWinGeoProc.Globals.Vertex[][] polyVertArray = new MapWinGeoProc.Globals.Vertex[numParts][];
                        MapWinGeoProc.Globals.ConvertPolyToVertexArray(ref polygon, out polyVertArray);

                        //check each point in the line to see if the entire line is either
                        //inside of the polygon or outside of it (we know it's inside polygon bounding box).
                        for (int i = 0; i <= numPoints - 1; i++)
                        {
                            currPt = line.get_Point(i);

                            //if(utils.PointInPolygon(polygon, currPt) == true)
                            if (MapWinGeoProc.Utils.PointInPolygon(ref polygon, ref currPt))////edited  april 28 2014
                            {
                                ptsInside[i] = true;
                                numInside += 1;
                            }
                            else
                            {
                                ptsInside[i] = false;
                                numOutside += 1;
                            }
                        }

                        //case: all points are inside polygon - check for possible intersections
                        if (numInside == numPoints)
                        {
                            if (ProcessAllInside(ref line, ref polygon, ref resultFile) == false)
                            {
                                gErrorMsg = "Problem processing inside line points: " + MapWinGeoProc.Error.GetLastErrorMsg();
                                Debug.WriteLine(gErrorMsg);
                                MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                                resultSF = resultFile;
                                return false;
                            }
                        }
                        //case: all points are outside of the polygon - check for possible intersections
                        else if (numOutside == numPoints)
                        {
                            if (ProcessAllOutside(ref line, ref polygon, ref resultFile) == false)
                            {
                                gErrorMsg = "Problem processing outside line points: " + MapWinGeoProc.Error.GetLastErrorMsg();
                                Debug.WriteLine(gErrorMsg);
                                MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                                resultSF = resultFile;
                                return false;
                            }
                        }
                        //case: part of line is inside and part is outside - find inside segments.
                        else
                        {
                            //if (ProcessPartInAndOut(ref ptsInside, ref line, ref polygon, ref resultFile) == false)
                            //{
                            //    gErrorMsg = "Problem processing part in and out line: " + MapWinGeoProc.Error.GetLastErrorMsg();
                            //    Debug.WriteLine(gErrorMsg);
                            //    MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                            //    resultSF = resultFile;
                            //    return false;
                            //}
                        }


                    }

                    //output result file, do not save to disk.
                    resultSF = resultFile;
                }
                else
                {
                    gErrorMsg = "Invalid shapefile type, should be of type polygon.";
                    Debug.WriteLine(gErrorMsg);
                    MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                    resultSF = resultFile;
                    return false;
                }
            }
            else //polygon or line is invalid
            {
                gErrorMsg = "Invalid object, cannot pass a NULL parameter.";
                Debug.WriteLine(gErrorMsg);
                MapWinGeoProc.Error.SetErrorMsg(gErrorMsg);
                resultSF = resultFile;
                return false;
            }
            return true;
        }
        #endregion

        /// <summary>
        /// Calculates the determinant of a 3X3 matrix, where the first two rows
        /// represent the x,y values of two lines, and the third row is (1 1 1).
        /// </summary>
        /// <param name="Elem11">The first element of the first row in the matrix.</param>
        /// <param name="Elem12">The second element of the first row in the matrix.</param>
        /// <param name="Elem13">The third element of the first row in the matrix.</param>
        /// <param name="Elem21">The first element of the second row in the matrix.</param>
        /// <param name="Elem22">The second element of the second row in the matrix.</param>
        /// <param name="Elem23">The third element of the second row in the matrix.</param>
        /// <returns>The determinant of the matrix.</returns>
        private static double TurboDeterm(double Elem11, double Elem12, double Elem13,
            double Elem21, double Elem22, double Elem23)
        {
            // The third row of the 3x3 matrix is (1,1,1)
            return Elem11 * (Elem22 - Elem23)
                - Elem12 * (Elem21 - Elem23)
                + Elem13 * (Elem21 - Elem22);
        }

        #region ProcessPartInAndOut

        /// <summary>
        /// Given a line that contains portions both inside and outside of the polygon, this
        /// function will split the polygon based only on the segments that completely bisect
        /// the polygon. The possibility of mutliple intersections for any 2pt segment is taken
        /// into account.
        /// </summary>
        /// <param name="insidePts">A boolean array indicating if a point is inside the polygon or not.</param>
        /// <param name="line">The line that intersects the polygon.</param>
        /// <param name="polygon">The polygon that will be split by the intersecting line.</param>
        /// <param name="resultSF">The shapefile that the polygon sections will be saved to.</param>
        /// <returns>False if errors were encountered or an assumption violated, true otherwise.</returns>
        //private static bool ProcessPartInAndOut(ref bool[] insidePts, ref MapWinGIS.Shape line, ref MapWinGIS.Shape polygon, ref MapWinGIS.Shapefile resultSF)
        //{
        //    int numLinePts = line.numPoints;
        //    int numLineSegs = numLinePts - 1;
        //    int numPolyPts = polygon.numPoints;
        //    int[] intersectsPerSeg = new int[numLineSegs];
        //    MapWinGIS.Point[][] intersectPts = new MapWinGIS.Point[numLineSegs][];
        //    int[][] polyIntLocs = new int[numLineSegs][]; //intersection occurs between polygon point indexed by polyIntLoc[][] and the previous point.

        //    //cut line into 2pt segments and put in new shapefile.
        //    ////IFeatureSet lineSegSF = new FeatureSet(FeatureTypes.Line);
        //    //MapWinGIS.Shapefile lineSegSF = new MapWinGIS.ShapefileClass();
        //    ////IFeature lineSegment;
        //    //IList<Coordinate> coordi = line.Coordinates;
        //    //Coordinate[] secCoordinate = new Coordinate[2];
        //    //cut line into 2pt segments and put in new shapefile.
        //    string tempPath = System.IO.Path.GetTempPath() + "tempLineSF.shp";
        //    MapWinGeoProc.DataManagement.DeleteShapefile(ref tempPath);
        //    MapWinGIS.Shapefile lineSegSF = new MapWinGIS.ShapefileClass();
        //    //CDM 8/4/2006 lineSegSF.CreateNew(tempPath, line.ShapeType);
        //    MapWinGeoProc.Globals.PrepareResultSF(ref tempPath, ref lineSegSF, line.ShapeType);
        //    int shpIndex = 0;
        //    MapWinGIS.Shape lineSegment;
        //    for (int i = 0; i <= numLineSegs - 1; i++)
        //    {
        //        //secCoordinate[0] = coordi[i];
        //        //secCoordinate[1] = coordi[i + 1];
        //        //lineSegment = new Feature(FeatureTypes.Line, secCoordinate);
        //        //lineSegSF.Features.Add(lineSegment);
        //        //lineSegment.Coordinates.Clear();
        //        lineSegment = new MapWinGIS.ShapeClass();
        //        lineSegment.ShapeType = line.ShapeType;
        //        int ptIndex = 0;
        //        lineSegment.InsertPoint(line.get_Point(i), ref ptIndex);
        //        ptIndex = 1;
        //        lineSegment.InsertPoint(line.get_Point(i + 1), ref ptIndex);
        //        shpIndex = lineSegSF.NumShapes;
        //        lineSegSF.EditInsertShape(lineSegment, ref shpIndex);

        //        intersectPts[i] = new MapWinGIS.Point[numPolyPts];
        //        polyIntLocs[i] = new int[numPolyPts];

        //    }
        //    //find number of intersections, intersection pts, and locations for each 2pt segment
        //    int numIntersects = 4;// CalcSiDeterm(ref lineSegSF, ref polygon, ref intersectsPerSeg, ref intersectPts, ref polyIntLocs);

        //    if (numIntersects == 0)
        //    {
        //        return false;
        //    }

        //    MapWinGIS.Shape insideLine = new MapWinGIS.Shape();
        //    List<Coordinate> insideLineList;
        //    MapWinGIS.Shape intersectSeg;
        //    List<Coordinate> intersectSegList;
        //    MapWinGIS.Point startIntersect = new MapWinGIS.Point();
        //    bool startIntExists = false;
        //    bool validInsideLine = false;
        //    int insideStart = 0;
        //    int startIntPolyLoc = 0;

        //    //loop through each 2pt segment
        //    for (int i = 0; i <= numLinePts - 2; i++)
        //    {
        //        insideLineList = new List<Coordinate>();
        //        lineSegment = lineSegSF.Features[i];
        //        int numSegIntersects = intersectsPerSeg[i];
        //        //****************** case: inside->inside **************************************//
        //        int ptIndex;
        //        if (insidePts[i] && insidePts[i + 1])
        //        {
        //            if (numSegIntersects == 0 && i != numLinePts - 2 && i != 0)
        //            {
        //                //add points to an inside line segment
        //                if (startIntExists)
        //                {
        //                    ptIndex = 0;
        //                    insideLineList.Insert(ptIndex, startIntersect..Coordinate);
        //                    startIntExists = false;
        //                    validInsideLine = true;
        //                    insideStart = startIntPolyLoc;
        //                }
        //                if (validInsideLine)
        //                {
        //                    ptIndex = insideLineList.Count;
        //                    insideLineList.Insert(ptIndex, lineSegment.Coordinates[0]);
        //                }
        //            }
        //            else
        //            {
        //                //sort the intersects and their locations
        //                Point[] intPts = new Point[numSegIntersects];
        //                Point startPt = new Point(lineSegSF.Features[i].Coordinates[0]);
        //                FindAndSortValidIntersects(numSegIntersects, ref intersectPts[i], ref intPts, ref startPt, ref polyIntLocs[i]);

        //                for (int j = 0; j <= numSegIntersects - 1; j++)
        //                {
        //                    if (j == 0)
        //                    {
        //                        if (startIntExists)
        //                        {
        //                            //first intersect pt is an ending pt, it must be
        //                            //combined with a starting intersect pt in order to section the polygon.

        //                            intersectSegList = new List<Coordinate>();
        //                            ptIndex = 0;
        //                            intersectSegList.Insert(ptIndex, startIntersect.Coordinate);
        //                            ptIndex = 1;
        //                            intersectSegList.Insert(ptIndex, lineSegment.Coordinates[0]);
        //                            ptIndex = 2;
        //                            intersectSegList.Insert(ptIndex, intPts[0].Coordinate);
        //                            intersectSeg = new Feature(FeatureTypes.Line, intersectSegList);

        //                            int firstPolyLoc = startIntPolyLoc;
        //                            int lastPolyLoc = polyIntLocs[i][0] - 1;
        //                            if (SectionPolygonWithLine(ref intersectSeg, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
        //                            {
        //                                return false;
        //                            }
        //                            startIntExists = false; //we used it up!
        //                        }
        //                        else if (insideLine.NumPoints != 0 && validInsideLine)
        //                        {
        //                            ptIndex = insideLineList.Count;
        //                            insideLineList.Insert(ptIndex, lineSegment.Coordinates[0]);
        //                            ptIndex++;
        //                            insideLineList.Insert(ptIndex, intPts[0].Coordinate);
        //                            insideLine = new Feature(FeatureTypes.Line, insideLineList);

        //                            int firstPolyLoc = insideStart;
        //                            int lastPolyLoc = polyIntLocs[i][0] - 1;
        //                            if (SectionPolygonWithLine(ref insideLine, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
        //                            {
        //                                return false;
        //                            }

        //                            validInsideLine = false;
        //                            insideLine.Coordinates.Clear();
        //                        }
        //                    }
        //                    else if (j == numSegIntersects - 1 && i != numLinePts - 2)
        //                    {
        //                        //last intersect pt is a starting pt, it must be
        //                        //saved for later combination
        //                        startIntersect = intPts[j];
        //                        startIntPolyLoc = polyIntLocs[i][j] - 1;
        //                        startIntExists = true;
        //                    }
        //                    else if (j != 0 || j != numSegIntersects - 1)
        //                    {
        //                        //a full poly section is created by two intersect points

        //                        intersectSegList = new List<Coordinate>();
        //                        ptIndex = intersectSegList.Count;
        //                        intersectSegList.Insert(ptIndex, intPts[j].Coordinate);
        //                        ptIndex++;
        //                        intersectSegList.Insert(ptIndex, intPts[j + 1].Coordinate);
        //                        intersectSeg = new Feature(FeatureTypes.Line, intersectSegList);

        //                        int firstPolyLoc = polyIntLocs[i][j] - 1;
        //                        int lastPolyLoc = polyIntLocs[i][j + 1] - 1;
        //                        if (SectionPolygonWithLine(ref intersectSeg, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
        //                        {
        //                            return false;
        //                        }
        //                        j++;
        //                    }
        //                }
        //            }

        //        }
        //        //********************** case: inside->outside ****************************************
        //        else if (insidePts[i] && insidePts[i + 1] == false)
        //        {
        //            if (numSegIntersects == 0)
        //            {
        //                return false;
        //            }
        //            if (numSegIntersects == 1)
        //            {
        //                if (startIntExists)
        //                {
        //                    intersectSegList = new List<Coordinate>();
        //                    ptIndex = 0;
        //                    intersectSegList.Insert(ptIndex, startIntersect.Coordinate);
        //                    ptIndex = 1;
        //                    intersectSegList.Insert(ptIndex, lineSegment.Coordinates[0]);
        //                    ptIndex = 2;
        //                    intersectSegList.Insert(ptIndex, intersectPts[i][0].Coordinate);
        //                    intersectSeg = new Feature(FeatureTypes.Line, intersectSegList);

        //                    int firstPolyLoc = startIntPolyLoc;
        //                    int lastPolyLoc = polyIntLocs[i][0] - 1;
        //                    if (SectionPolygonWithLine(ref intersectSeg, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
        //                    {
        //                        return false;
        //                    }

        //                    startIntExists = false; //we just used it up!
        //                }
        //                else if (insideLine.NumPoints != 0 && validInsideLine)
        //                {
        //                    ptIndex = insideLineList.Count;
        //                    insideLineList.Insert(ptIndex, lineSegment.Coordinates[0]);
        //                    ptIndex++;
        //                    insideLineList.Insert(ptIndex, intersectPts[i][0].Coordinate);
        //                    insideLine = new Feature(FeatureTypes.Line, insideLineList);

        //                    int firstPolyLoc = insideStart;
        //                    int lastPolyLoc = polyIntLocs[i][0] - 1;
        //                    if (SectionPolygonWithLine(ref insideLine, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
        //                    {
        //                        return false;
        //                    }

        //                    validInsideLine = false;
        //                    insideLine.Coordinates.Clear();
        //                }
        //            }
        //            else
        //            {
        //                //sort the intersects and their locations
        //                Point[] intPts = new Point[numSegIntersects];
        //                Point startPt = new Point(lineSegSF.Features[i].Coordinates[0]);
        //                FindAndSortValidIntersects(numSegIntersects, ref intersectPts[i], ref intPts, ref startPt, ref polyIntLocs[i]);
        //                for (int j = 0; j <= numSegIntersects - 1; j++)
        //                {
        //                    if (j == 0)
        //                    {
        //                        if (startIntExists)
        //                        {
        //                            intersectSegList = new List<Coordinate>();
        //                            ptIndex = 0;
        //                            intersectSegList.Insert(ptIndex, startIntersect.Coordinate);
        //                            ptIndex = 1;
        //                            intersectSegList.Insert(ptIndex, lineSegment.Coordinates[0]);
        //                            ptIndex = 2;
        //                            intersectSegList.Insert(ptIndex, intPts[0].Coordinate);
        //                            intersectSeg = new Feature(FeatureTypes.Line, intersectSegList);

        //                            int firstPolyLoc = startIntPolyLoc;
        //                            int lastPolyLoc = polyIntLocs[i][0] - 1;
        //                            if (SectionPolygonWithLine(ref intersectSeg, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
        //                            {
        //                                return false;
        //                            }

        //                            startIntExists = false; //we just used it up!
        //                        }
        //                        else if (insideLine.NumPoints != 0 && validInsideLine)
        //                        {
        //                            ptIndex = insideLineList.Count;
        //                            insideLineList.Insert(ptIndex, lineSegment.Coordinates[0]);
        //                            ptIndex++;
        //                            insideLineList.Insert(ptIndex, intPts[0].Coordinate);
        //                            insideLine = new Feature(FeatureTypes.Line, insideLineList);

        //                            int firstPolyLoc = insideStart;
        //                            int lastPolyLoc = polyIntLocs[i][0] - 1;
        //                            if (SectionPolygonWithLine(ref insideLine, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
        //                            {
        //                                return false;
        //                            }

        //                            validInsideLine = false;
        //                            insideLine.Coordinates.Clear();
        //                        }
        //                    }
        //                    else
        //                    {
        //                        //section the polygon with the intersecting segment
        //                        //intersectSeg = new Feature();
        //                        intersectSegList = new List<Coordinate>();
        //                        ptIndex = 0;
        //                        intersectSegList.Insert(ptIndex, intPts[j].Coordinate);
        //                        ptIndex = 1;
        //                        intersectSegList.Insert(ptIndex, intPts[j + 1].Coordinate);
        //                        intersectSeg = new Feature(FeatureTypes.Line, intersectSegList);

        //                        intersectSeg.Coordinates.Insert(ptIndex, intPts[j + 1].Coordinate);
        //                        int firstPolyLoc = polyIntLocs[i][j] - 1;
        //                        int lastPolyLoc = polyIntLocs[i][j + 1] - 1;
        //                        if (SectionPolygonWithLine(ref intersectSeg, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
        //                        {
        //                            return false;
        //                        }
        //                        j++;
        //                    }
        //                }
        //            }
        //        }
        //        //********************** case: outside->inside ***************************************
        //        else if (insidePts[i] == false && insidePts[i + 1])
        //        {
        //            validInsideLine = false;
        //            startIntExists = false;

        //            if (numSegIntersects == 0)
        //            {
        //                return false;
        //            }
        //            if (numSegIntersects == 1)
        //            {
        //                startIntExists = true;
        //                startIntersect = intersectPts[i][0];
        //                startIntPolyLoc = polyIntLocs[i][0] - 1;
        //            }
        //            else
        //            {
        //                //sort the intersects and their locations
        //                Point[] intPts = new Point[numSegIntersects];
        //                Point startPt = new Point(lineSegSF.Features[i].Coordinates[0]);
        //                FindAndSortValidIntersects(numSegIntersects, ref intersectPts[i], ref intPts, ref startPt, ref polyIntLocs[i]);

        //                //an odd number of intersects exist, at least one full poly section
        //                //will be created along with one hanging intersect pt.
        //                for (int j = 0; j <= numSegIntersects - 1; j++)
        //                {
        //                    if (j == numSegIntersects - 1)
        //                    {
        //                        startIntExists = true;
        //                        startIntersect = intPts[j];
        //                        startIntPolyLoc = polyIntLocs[i][j] - 1;
        //                    }
        //                    else
        //                    {
        //                        intersectSegList = new List<Coordinate>();
        //                        ptIndex = 0;
        //                        intersectSegList.Insert(ptIndex, intPts[j].Coordinate);
        //                        ptIndex = 1;
        //                        intersectSegList.Insert(ptIndex, intPts[j + 1].Coordinate);
        //                        intersectSeg = new Feature(FeatureTypes.Line, intersectSegList);

        //                        int firstPolyLoc = polyIntLocs[i][j] - 1;
        //                        int lastPolyLoc = polyIntLocs[i][j + 1] - 1;
        //                        if (SectionPolygonWithLine(ref intersectSeg, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
        //                        {
        //                            return false;
        //                        }
        //                        j++;
        //                    }
        //                }
        //            }
        //        }
        //        //************************ case: outside->outside ***********************************
        //        else if (insidePts[i] == false && insidePts[i + 1] == false)
        //        {
        //            startIntExists = false;
        //            validInsideLine = false;

        //            if (numSegIntersects == 0)
        //            {
        //                //do nothing
        //            }
        //            else
        //            {
        //                //sort the intersects and their locations
        //                MapWinGIS.Point[] intPts = new MapWinGIS.Point[numSegIntersects];
        //                MapWinGIS.Point startPt = new MapWinGIS.Point(lineSegSF.Features[i].Coordinates[0]);
        //                FindAndSortValidIntersects(numSegIntersects, ref intersectPts[i], ref intPts, ref startPt, ref polyIntLocs[i]);

        //                //should always be an even amount of intersections, full poly section created
        //                for (int j = 0; j <= numSegIntersects - 1; j++)
        //                {
        //                    intersectSegList = new List<Coordinate>();
        //                    ptIndex = 0;
        //                    intersectSegList.Insert(ptIndex, intPts[j].Coordinate);
        //                    ptIndex = 1;
        //                    intersectSegList.Insert(ptIndex, intPts[j + 1].Coordinate);
        //                    intersectSeg = new Feature(FeatureTypes.Line, intersectSegList);

        //                    int firstPolyLoc = polyIntLocs[i][j] - 1;
        //                    int lastPolyLoc = polyIntLocs[i][j + 1] - 1;
        //                    if (SectionPolygonWithLine(ref intersectSeg, ref polygon, firstPolyLoc, lastPolyLoc, ref resultSF) == false)
        //                    {
        //                        return false;
        //                    }
        //                    j++;
        //                }
        //            }
        //        }
        //    }
        //    return true;
        //}
        #endregion

        public void grlegend()
        {
            Cursor.Current = Cursors.WaitCursor;
            setclickdefault();

            Shapefile sf = (Shapefile)frmM.axMap.get_GetObject(frmM.hndMapSel);

            Table sftable = sf.Table;
            int fldidx = -1;

            if (sf.ShapefileType == ShpfileType.SHP_POLYGON)
            {
                if ((frmM.legend1.Map.get_LayerName(frmM.hndMapSel).ToUpper().Contains("BARANGAY")) && (frmM.legend1.Map.get_LayerName(frmM.hndMapSel).ToUpper() != ("BARANGAY BOUNDARY")))
                {
                    //to do set the fieldnames as dymanic
                    fldidx = sftable.get_FieldIndexByName("DIST_CODE");
                    string strval = String.Empty;
                    if (fldidx != -1)
                    {
                        if (sf.Table.NumRows > 0)
                        {
                            int cntr = 0;
                            do
                            {
                                strval = sf.get_CellValue(fldidx, cntr).ToString().Trim();
                                cntr += 1;
                                if (sf.Table.NumRows == cntr)
                                    break;
                            } while (sf.get_CellValue(fldidx, cntr).ToString().Trim().Length > 0);
                        }
                    }

                    sf.DefaultDrawingOptions.FillVisible = true;
                    sf.Categories.Clear();

                    //set category
                    //for setting the fill values
                    sf.DefaultDrawingOptions.FillTransparency = (float)255;
                    sf.DefaultDrawingOptions.FillType = tkFillType.ftStandard; //tkFillType.ftGradient;
                    sf.DefaultDrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Brown));
                    sf.DefaultDrawingOptions.FillBgTransparent = false;
                    //for setting the outline values
                    sf.DefaultDrawingOptions.LineColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.LimeGreen));
                    sf.DefaultDrawingOptions.LineTransparency = (float)255;
                    sf.DefaultDrawingOptions.LineWidth = 1;

                    sf.Categories.Generate(fldidx, tkClassificationType.ctUniqueValues, 0);

                    for (int i = 0; i < sf.Categories.Count; i++)
                    {
                        ShapefileCategory cat = sf.Categories.get_Item(i);
                        if (cat.Name.Trim() == "")
                        {
                            cat.DrawingOptions.FillVisible = false;
                            cat.DrawingOptions.FillColor = (uint)System.Drawing.ColorTranslator.ToOle(Color.Black);
                            cat.DrawingOptions.LineVisible = true;
                            cat.DrawingOptions.LineColor = (uint)System.Drawing.ColorTranslator.ToOle(Color.LimeGreen);
                            cat.Name = "W/O GR-VALUE";
                        }
                        else if (cat.Name.Trim() == frmM.c_lgudist)
                        {
                            cat.DrawingOptions.FillVisible = true;
                            cat.DrawingOptions.FillColor = (uint)System.Drawing.ColorTranslator.ToOle(Color.Violet);
                            cat.DrawingOptions.LineVisible = true;
                            cat.DrawingOptions.LineColor = (uint)System.Drawing.ColorTranslator.ToOle(Color.LimeGreen);
                            cat.Name = "WITH GR-VALUE";
                        }
                    }
                    sf.Categories.Caption = "LAND: GENERAL REVISION";
                    sf.DefaultDrawingOptions.LineVisible = true;
                    sf.DefaultDrawingOptions.VerticesVisible = false;
                    sf.DefaultDrawingOptions.FillVisible = false;

                    frmM.legend1.Layers.ItemByHandle(frmM.hndMapSel).Refresh();
                    frmM.axMap.Redraw();
                    frmM.axMap.Refresh();

                    Cursor.Current = Cursors.Default;
                }
            }
        }

        public void grlegend2()
        {
            Cursor.Current = Cursors.WaitCursor;
            setclickdefault();

            Shapefile sf = (Shapefile)frmM.axMap.get_GetObject(frmM.hndMapSel);

            Table sftable = sf.Table;
            int fldidx = -1;
            int pinfldidx = -1;
            string strpinval = String.Empty;
            Table m_shapefiletable = sf.Table;
            string slayerdist = String.Empty;
            string slayerbrgy = String.Empty;
            string slayercity = String.Empty;

            if (sf.ShapefileType == ShpfileType.SHP_POLYGON)
            {
                if ((frmM.legend1.Map.get_LayerName(frmM.hndMapSel).ToUpper().Contains("BARANGAY")) && (frmM.legend1.Map.get_LayerName(frmM.hndMapSel).ToUpper() != ("BARANGAY BOUNDARY")))
                {
                    //to do set the fieldnames as dymanic
                    fldidx = sftable.get_FieldIndexByName("DIST_CODE");
                    pinfldidx = sftable.get_FieldIndexByName("PIN");

                    string strval = String.Empty;
                    if (fldidx != -1)
                    {
                        if (sf.Table.NumRows > 0)
                        {
                            int cntr = 0;
                            do
                            {
                                strval = sf.get_CellValue(fldidx, cntr).ToString().Trim();
                                cntr += 1;
                                if (sf.Table.NumRows == cntr)
                                    break;
                            } while (sf.get_CellValue(fldidx, cntr).ToString().Trim().Length > 0);
                        }
                    }

                    if (pinfldidx != -1)
                    {
                        if (sf.Table.NumRows > 0)
                        {
                            int cntr = 0;
                            do
                            {
                                strpinval = sf.get_CellValue(pinfldidx, cntr).ToString().Trim();
                                cntr += 1;
                                if (sf.Table.NumRows == cntr)
                                    break;
                            } while (sf.get_CellValue(pinfldidx, cntr).ToString().Trim().Length > 0);

                            slayerbrgy = strpinval.Substring(7, 3);
                            slayercity = strpinval.Substring(0, 3);
                            slayerdist = strpinval.Substring(4, 2);
                        }
                    }

                    bool result = sf.StartEditingShapes(true, null);
                    fldidx = m_shapefiletable.get_FieldIndexByName("DIST_CODE");
                    if (fldidx == -1)
                    {
                        MapWinGIS.Field fld = new MapWinGIS.FieldClass();
                        fld.Key = "DIST_CODE";
                        fld.Name = "DIST_CODE";
                        fld.Width = 2;
                        fld.Type = MapWinGIS.FieldType.STRING_FIELD;
                        int intref = m_shapefiletable.NumFields;
                        sf.EditInsertField(fld, ref intref, null);
                    }
                    else
                        setflddefaultval(sf, "DIST_CODE");

                    StringBuilder sqry = new StringBuilder();
                    sqry.Append(String.Format("SELECT PIN,DIST_CODE FROM GIS_LAND_SKED WHERE PIN LIKE '{0}%'", slayercity + "-" + slayerdist + "-" + slayerbrgy));

                    OleDbConnection oledbconn = new OleDbConnection(frmM.m_strconn);
                    OleDbCommand cmd = new OleDbCommand(sqry.ToString(), oledbconn);
                    OleDbDataAdapter newDataAdapter = new OleDbDataAdapter(cmd);
                    DataSet newDataSet = new DataSet();
                    oledbconn.Open();
                    OleDbDataReader newDataReaders = cmd.ExecuteReader();


                    if (newDataReaders.HasRows == true)
                    {
                        while (newDataReaders.Read())
                        {
                            string strPinNo = String.Empty;
                            int shpidx = -1;
                            if (!DBNull.Value.Equals(newDataReaders["PIN"]))
                            {
                                strPinNo = newDataReaders["PIN"].ToString().Trim().ToUpper();
                            }
                            for (int i = 0; i < sf.NumShapes; i++)
                            {
                                if (strPinNo == sf.get_CellValue(pinfldidx, i).ToString().ToUpper().Trim())
                                {
                                    shpidx = i;
                                    break;
                                }
                            }
                            if (!DBNull.Value.Equals(newDataReaders["DIST_CODE"]))
                            {
                                string strval1 = String.Empty;
                                if (shpidx > -1)
                                {
                                    strval1 = newDataReaders["DIST_CODE"].ToString().Trim().ToUpper();
                                    if (strval1 != String.Empty)
                                    {
                                        sf.EditCellValue(fldidx, shpidx, strval1.ToUpper().Trim());
                                    }
                                }
                            }
                        }
                    }
                    result = sf.StopEditingShapes(true, true, null);

                    sf.DefaultDrawingOptions.FillVisible = true;
                    sf.Categories.Clear();

                    if (File.Exists(Environment.CurrentDirectory.ToString() + @"\legend\land_general_revision.mwleg"))
                        frmM.LoadShapefileLayerLegend(Environment.CurrentDirectory.ToString() + @"\legend\land_general_revision.mwleg");
                    else
                    {
                        //set category
                        //for setting the fill values
                        sf.DefaultDrawingOptions.FillTransparency = (float)255;
                        sf.DefaultDrawingOptions.FillType = tkFillType.ftStandard; //tkFillType.ftGradient;
                        sf.DefaultDrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Brown));
                        sf.DefaultDrawingOptions.FillBgTransparent = false;
                        //for setting the outline values
                        sf.DefaultDrawingOptions.LineColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.LimeGreen));
                        sf.DefaultDrawingOptions.LineTransparency = (float)255;
                        sf.DefaultDrawingOptions.LineWidth = 1;

                        sf.Categories.Generate(fldidx, tkClassificationType.ctUniqueValues, 0);

                        for (int i = 0; i < sf.Categories.Count; i++)
                        {
                            ShapefileCategory cat = sf.Categories.get_Item(i);
                            if (cat.Name.Trim() == "")
                            {
                                cat.DrawingOptions.FillVisible = false;
                                cat.DrawingOptions.FillColor = (uint)System.Drawing.ColorTranslator.ToOle(Color.Black);
                                cat.DrawingOptions.LineVisible = true;
                                cat.DrawingOptions.LineColor = (uint)System.Drawing.ColorTranslator.ToOle(Color.LimeGreen);
                                cat.Name = "W/O GR-VALUE";
                            }
                            else if (cat.Name.Trim() == frmM.c_lgudist)
                            {
                                cat.DrawingOptions.FillVisible = true;
                                cat.DrawingOptions.FillColor = (uint)System.Drawing.ColorTranslator.ToOle(Color.Violet);
                                cat.DrawingOptions.LineVisible = true;
                                cat.DrawingOptions.LineColor = (uint)System.Drawing.ColorTranslator.ToOle(Color.LimeGreen);
                                cat.Name = "WITH GR-VALUE";
                            }
                        }
                        sf.Categories.Caption = "LAND: GENERAL REVISION";
                        sf.DefaultDrawingOptions.LineVisible = true;
                        sf.DefaultDrawingOptions.VerticesVisible = false;
                        sf.DefaultDrawingOptions.FillVisible = false;
                    }

                    frmM.legend1.Layers.ItemByHandle(frmM.hndMapSel).Refresh();
                    frmM.axMap.Redraw();
                    frmM.axMap.Refresh();

                    Cursor.Current = Cursors.Default;
                }
            }
        }

        public void grclsuvallegend()
        {
            Cursor.Current = Cursors.WaitCursor;
            setclickdefault();

            Shapefile sf = (Shapefile)frmM.axMap.get_GetObject(frmM.hndMapSel);

            Table sftable = sf.Table;
            int fldidx = -1;

            if (sf.ShapefileType == ShpfileType.SHP_POLYGON)
            {
                if ((frmM.legend1.Map.get_LayerName(frmM.hndMapSel).ToUpper().Contains("BARANGAY")) && (frmM.legend1.Map.get_LayerName(frmM.hndMapSel).ToUpper() != ("BARANGAY BOUNDARY")))
                {
                    //to do set the fieldnames as dymanic
                    fldidx = sftable.get_FieldIndexByName("SCLS_UVAL");
                    string strval = String.Empty;
                    if (fldidx != -1)
                    {
                        if (sf.Table.NumRows > 0)
                        {
                            int cntr = 0;
                            do
                            {
                                strval = sf.get_CellValue(fldidx, cntr).ToString().Trim();
                                cntr += 1;
                                if (sf.Table.NumRows == cntr)
                                    break;
                            } while (sf.get_CellValue(fldidx, cntr).ToString().Trim().Length > 0);
                        }
                    }

                    sf.DefaultDrawingOptions.FillVisible = true;
                    sf.Categories.Clear();

                    //set category
                    //for setting the fill values
                    sf.DefaultDrawingOptions.FillTransparency = (float)255;
                    sf.DefaultDrawingOptions.FillType = tkFillType.ftStandard; //tkFillType.ftGradient;
                    sf.DefaultDrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Brown));
                    sf.DefaultDrawingOptions.FillBgTransparent = false;
                    //for setting the outline values
                    sf.DefaultDrawingOptions.LineColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.LimeGreen));
                    sf.DefaultDrawingOptions.LineTransparency = (float)255;
                    sf.DefaultDrawingOptions.LineWidth = 1;

                    sf.Categories.Generate(fldidx, tkClassificationType.ctUniqueValues, 0);

                    for (int i = 0; i < sf.Categories.Count; i++)
                    {
                        ShapefileCategory cat = sf.Categories.get_Item(i);
                        if (cat.Name.Trim() == "")
                        {
                            cat.DrawingOptions.FillVisible = false;
                            cat.DrawingOptions.FillColor = (uint)System.Drawing.ColorTranslator.ToOle(Color.Black);
                            cat.DrawingOptions.LineVisible = true;
                            cat.DrawingOptions.LineColor = (uint)System.Drawing.ColorTranslator.ToOle(Color.LimeGreen);
                            cat.Name = "NO VALUE";
                        }
                    }
                    //GENERATE color legend
                    ColorScheme scheme = new ColorScheme();
                    scheme.SetColors2(tkMapColor.DarkGreen, tkMapColor.Khaki);
                    sf.Categories.ApplyColorScheme(tkColorSchemeType.ctSchemeGraduated, scheme);

                    sf.Categories.Caption = "LAND: GENERAL REVISION";
                    sf.DefaultDrawingOptions.LineVisible = true;
                    sf.DefaultDrawingOptions.VerticesVisible = false;
                    sf.DefaultDrawingOptions.FillVisible = false;

                    frmM.legend1.Layers.ItemByHandle(frmM.hndMapSel).Refresh();
                    frmM.axMap.Redraw();
                    frmM.axMap.Refresh();

                    Cursor.Current = Cursors.Default;
                }
            }
        }

        public void grclsuvallegend2()
        {
            Cursor.Current = Cursors.WaitCursor;
            setclickdefault();

            Shapefile sf = (Shapefile)frmM.axMap.get_GetObject(frmM.hndMapSel);

            Table sftable = sf.Table;
            int fldidx = -1;
            int pinfldidx = -1;
            string strpinval = String.Empty;
            Table m_shapefiletable = sf.Table;
            string slayerdist = String.Empty;
            string slayerbrgy = String.Empty;
            string slayercity = String.Empty;

            if (sf.ShapefileType == ShpfileType.SHP_POLYGON)
            {
                if ((frmM.legend1.Map.get_LayerName(frmM.hndMapSel).ToUpper().Contains("BARANGAY")) && (frmM.legend1.Map.get_LayerName(frmM.hndMapSel).ToUpper() != ("BARANGAY BOUNDARY")))
                {
                    //to do set the fieldnames as dymanic
                    fldidx = sftable.get_FieldIndexByName("SCLS_UVAL");
                    pinfldidx = sftable.get_FieldIndexByName("PIN");

                    string strval = String.Empty;
                    if (fldidx != -1)
                    {
                        if (sf.Table.NumRows > 0)
                        {
                            int cntr = 0;
                            do
                            {
                                strval = sf.get_CellValue(fldidx, cntr).ToString().Trim();
                                cntr += 1;
                                if (sf.Table.NumRows == cntr)
                                    break;
                            } while (sf.get_CellValue(fldidx, cntr).ToString().Trim().Length > 0);
                        }
                    }

                    if (pinfldidx != -1)
                    {
                        if (sf.Table.NumRows > 0)
                        {
                            int cntr = 0;
                            do
                            {
                                strpinval = sf.get_CellValue(pinfldidx, cntr).ToString().Trim();
                                cntr += 1;
                                if (sf.Table.NumRows == cntr)
                                    break;
                            } while (sf.get_CellValue(pinfldidx, cntr).ToString().Trim().Length > 0);

                            slayerbrgy = strpinval.Substring(7, 3);
                            slayercity = strpinval.Substring(0, 3);
                            slayerdist = strpinval.Substring(4, 2);
                        }
                    }

                    bool result = sf.StartEditingShapes(true, null);
                    fldidx = m_shapefiletable.get_FieldIndexByName("SCLS_UVAL");
                    if (fldidx == -1)
                    {
                        MapWinGIS.Field fld = new MapWinGIS.FieldClass();
                        fld.Key = "SCLS_UVAL";
                        fld.Name = "SCLS_UVAL";
                        fld.Width = 250;
                        fld.Type = MapWinGIS.FieldType.STRING_FIELD;
                        int intref = m_shapefiletable.NumFields;
                        sf.EditInsertField(fld, ref intref, null);
                    }
                    else
                        setflddefaultval(sf, "SCLS_UVAL");

                    StringBuilder sqry = new StringBuilder();
                    sqry.Append(String.Format("SELECT PIN,CLASS||'-'||SUBCLASS||'_'||UVAL AS SCLS_UVAL FROM GIS_LAND_SKED WHERE PIN LIKE '{0}%'", slayercity + "-" + slayerdist + "-" + slayerbrgy));

                    OleDbConnection oledbconn = new OleDbConnection(frmM.m_strconn);
                    OleDbCommand cmd = new OleDbCommand(sqry.ToString(), oledbconn);
                    OleDbDataAdapter newDataAdapter = new OleDbDataAdapter(cmd);
                    DataSet newDataSet = new DataSet();
                    oledbconn.Open();
                    OleDbDataReader newDataReaders = cmd.ExecuteReader();


                    if (newDataReaders.HasRows == true)
                    {
                        while (newDataReaders.Read())
                        {
                            string strPinNo = String.Empty;
                            int shpidx = -1;
                            if (!DBNull.Value.Equals(newDataReaders["PIN"]))
                            {
                                strPinNo = newDataReaders["PIN"].ToString().Trim().ToUpper();
                            }
                            for (int i = 0; i < sf.NumShapes; i++)
                            {
                                if (strPinNo == sf.get_CellValue(pinfldidx, i).ToString().ToUpper().Trim())
                                {
                                    shpidx = i;
                                    break;
                                }
                            }
                            if (!DBNull.Value.Equals(newDataReaders["SCLS_UVAL"]))
                            {
                                string strval1 = String.Empty;
                                if (shpidx > -1)
                                {
                                    strval1 = newDataReaders["SCLS_UVAL"].ToString().Trim().ToUpper();
                                    if (strval1 != String.Empty)
                                    {
                                        sf.EditCellValue(fldidx, shpidx, strval1.ToUpper().Trim());
                                    }
                                }
                            }
                        }
                    }
                    result = sf.StopEditingShapes(true, true, null);

                    sf.DefaultDrawingOptions.FillVisible = true;
                    sf.Categories.Clear();

                    if (File.Exists(Environment.CurrentDirectory.ToString() + @"\legend\land_gr_cls_subcls_uval.mwleg"))
                        frmM.LoadShapefileLayerLegend(Environment.CurrentDirectory.ToString() + @"\legend\land_gr_cls_subcls_uval.mwleg");
                    else
                    {
                        //set category
                        //for setting the fill values
                        sf.DefaultDrawingOptions.FillTransparency = (float)255;
                        sf.DefaultDrawingOptions.FillType = tkFillType.ftStandard; //tkFillType.ftGradient;
                        sf.DefaultDrawingOptions.FillColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Brown));
                        sf.DefaultDrawingOptions.FillBgTransparent = false;
                        //for setting the outline values
                        sf.DefaultDrawingOptions.LineColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.LimeGreen));
                        sf.DefaultDrawingOptions.LineTransparency = (float)255;
                        sf.DefaultDrawingOptions.LineWidth = 1;

                        sf.Categories.Generate(fldidx, tkClassificationType.ctUniqueValues, 0);

                        for (int i = 0; i < sf.Categories.Count; i++)
                        {
                            ShapefileCategory cat = sf.Categories.get_Item(i);
                            if (cat.Name.Trim() == "")
                            {
                                cat.DrawingOptions.FillVisible = false;
                                cat.DrawingOptions.FillColor = (uint)System.Drawing.ColorTranslator.ToOle(Color.Black);
                                cat.DrawingOptions.LineVisible = true;
                                cat.DrawingOptions.LineColor = (uint)System.Drawing.ColorTranslator.ToOle(Color.LimeGreen);
                                cat.Name = "NO VALUE";
                            }
                        }
                        //GENERATE color legend
                        ColorScheme scheme = new ColorScheme();
                        scheme.SetColors2(tkMapColor.DarkGreen, tkMapColor.Khaki);
                        sf.Categories.ApplyColorScheme(tkColorSchemeType.ctSchemeGraduated, scheme);

                        sf.Categories.Caption = "LAND: GR-CLS-SUBCLS-UVAL";
                        sf.DefaultDrawingOptions.LineVisible = true;
                        sf.DefaultDrawingOptions.VerticesVisible = false;
                        sf.DefaultDrawingOptions.FillVisible = false;
                    }

                    frmM.legend1.Layers.ItemByHandle(frmM.hndMapSel).Refresh();
                    frmM.axMap.Redraw();
                    frmM.axMap.Refresh();

                    Cursor.Current = Cursors.Default;
                }
            }
        }

        public void checklabelregistry()
        {
            clsRegistry clsreg = new clsRegistry();
            bool flag;
            //land parcel
            if (Boolean.TryParse(clsreg.ReadDword("CurrentUser", @"Software\Amellar\PARCEL.NET\Layers\Land Parcel", "SetLabel"), out flag))
                frmM.m_shapelandlabelset = flag;
            else
                frmM.m_shapelandlabelset = false;
            if (Boolean.TryParse(clsreg.ReadDword("CurrentUser", @"Software\Amellar\PARCEL.NET\Layers\Land Parcel", "SetLayer"), out flag))
                frmM.m_shapelandlayerset = flag;
            else
                frmM.m_shapelandlayerset = false;

            //barangay boundary
            if (Boolean.TryParse(clsreg.ReadDword("CurrentUser", @"Software\Amellar\PARCEL.NET\Layers\Barangay Boundary", "SetLabel"), out flag))
                frmM.m_shapebrgylabelset = flag;
            else
                frmM.m_shapebrgylabelset = false;
            if (Boolean.TryParse(clsreg.ReadDword("CurrentUser", @"Software\Amellar\PARCEL.NET\Layers\Barangay Boundary", "SetLayer"), out flag))
                frmM.m_shapebrgylayerset = flag;
            else
                frmM.m_shapebrgylayerset = false;

            //section boundary
            if (Boolean.TryParse(clsreg.ReadDword("CurrentUser", @"Software\Amellar\PARCEL.NET\Layers\Section Boundary", "SetLabel"), out flag))
                frmM.m_shapesectlabelset = flag;
            else
                frmM.m_shapesectlabelset = false;
            if (Boolean.TryParse(clsreg.ReadDword("CurrentUser", @"Software\Amellar\PARCEL.NET\Layers\Section Boundary", "SetLayer"), out flag))
                frmM.m_shapesectlayerset = flag;
            else
                frmM.m_shapesectlayerset = false;

            //road network
            if (Boolean.TryParse(clsreg.ReadDword("CurrentUser", @"Software\Amellar\PARCEL.NET\Layers\Road Network", "SetLabel"), out flag))
                frmM.m_shaperoadnetlabelset = flag;
            else
                frmM.m_shaperoadnetlabelset = false;
            if (Boolean.TryParse(clsreg.ReadDword("CurrentUser", @"Software\Amellar\PARCEL.NET\Layers\Road Network", "SetLayer"), out flag))
                frmM.m_shaperoadnetlayerset = flag;
            else
                frmM.m_shaperoadnetlayerset = false;

            //building
            if (Boolean.TryParse(clsreg.ReadDword("CurrentUser", @"Software\Amellar\PARCEL.NET\Layers\Building", "SetLabel"), out flag))
                frmM.m_shapebldglabelset = flag;
            else
                frmM.m_shapebldglabelset = false;
            if (Boolean.TryParse(clsreg.ReadDword("CurrentUser", @"Software\Amellar\PARCEL.NET\Layers\Building", "SetLayer"), out flag))
                frmM.m_shapebldglayerset = flag;
            else
                frmM.m_shapebldglayerset = false;

            //landmarks
            if (Boolean.TryParse(clsreg.ReadDword("CurrentUser", @"Software\Amellar\PARCEL.NET\Layers\Landmarks", "SetLabel"), out flag))
                frmM.m_shapelandmarklabelset = flag;
            else
                frmM.m_shapelandmarklabelset = false;
            if (Boolean.TryParse(clsreg.ReadDword("CurrentUser", @"Software\Amellar\PARCEL.NET\Layers\Landmarks", "SetLayer"), out flag))
                frmM.m_shapelandmarklayerset = flag;
            else
                frmM.m_shapelandmarklayerset = false;

            //district boundary
            if (Boolean.TryParse(clsreg.ReadDword("CurrentUser", @"Software\Amellar\PARCEL.NET\Layers\District Boundary", "SetLabel"), out flag))
                frmM.m_shapedistlabelset = flag;
            else
                frmM.m_shapedistlabelset = false;
            if (Boolean.TryParse(clsreg.ReadDword("CurrentUser", @"Software\Amellar\PARCEL.NET\Layers\District Boundary", "SetLayer"), out flag))
                frmM.m_shapedistlayerset = flag;
            else
                frmM.m_shapedistlayerset = false;
        }


        public Shapefile setLabelRegistry(Shapefile sf, string slayer)
        {
            Shapefile resultsf = sf;
            string labelfield = String.Empty;
            string labelfont = String.Empty;
            string fontcolor = String.Empty;
            int intfontsize = -1;
            int ifontsize = -1;
            bool framevisible;
            bool fvisible;
            bool labelvisible;
            bool lvisible;
            tkLabelFrameType frametype = tkLabelFrameType.lfPointedRectangle;
            string framecolorleft = String.Empty;
            string framecolorright = String.Empty;
            bool removeduplicates;
            bool rdupli;
            bool avoidcollision;
            bool acollision;
            bool shadowvisible;
            bool svisible;
            bool halovisible;
            bool hvisible;
            bool dynamicvisible;
            bool dvisible;
            double maxvisiblescale = 0;
            double minvisiblescale = 0;
            //string labelalignment = String.Empty;
            tkLabelAlignment labelalignment = tkLabelAlignment.laCenter;

            clsRegistry clsreg = new clsRegistry();
            string strhive = "CurrentUser";
            string straddress = String.Format(@"Software\Amellar\PARCEL.NET\Layers\{0}\Label",slayer);
            Microsoft.Win32.RegistryKey baca = clsreg.Hive(strhive);
            Microsoft.Win32.RegistryKey baca1 = baca.OpenSubKey(straddress);
            if (baca1 == null)
                baca1 = baca.CreateSubKey(straddress);

            baca1 = baca.OpenSubKey(straddress);
            labelfield = clsreg.ReadDword(strhive, straddress, "LabelField");//the field name where the label will be based
            labelfont = clsreg.ReadDword(strhive, straddress, "LabelFont").ToUpper();// the default font of the label
            if (labelfont == "")
                labelfont = "ARIAL";
            fontcolor = clsreg.ReadDword(strhive, straddress, "FontColor");// the default font color of the label
            if (fontcolor == "")
                fontcolor = "White";
            if (Int32.TryParse(clsreg.ReadDword(strhive, straddress, "LabelFontSize"), out ifontsize))// the default font size of the label
                intfontsize = ifontsize;//Convert.ToInt32(clsreg.ReadDword(strhive, straddress, "LabelFontSize"));
            if (Boolean.TryParse(clsreg.ReadDword(strhive, straddress, "FrameVisible"), out fvisible))// the label frame visibility
                framevisible = fvisible;
            else
                framevisible = false;

            if (Boolean.TryParse(clsreg.ReadDword(strhive, straddress, "LabelVisible"), out lvisible))// the label visiblity
                labelvisible = lvisible;
            else
                labelvisible = false;

            if (Boolean.TryParse(clsreg.ReadDword(strhive, straddress, "ShadowVisible"), out svisible))// the shadow visiblity
                shadowvisible = svisible;
            else
                shadowvisible = false;

            if (Boolean.TryParse(clsreg.ReadDword(strhive, straddress, "HaloVisible"), out hvisible))// the halo visiblity
                halovisible = hvisible;
            else
                halovisible = false;

            ///the label frame shape
            if (clsreg.ReadDword(strhive, straddress, "FrameType").ToUpper() == "lfPointedRectangle".ToUpper())
                frametype = tkLabelFrameType.lfPointedRectangle;
            else if (clsreg.ReadDword(strhive, straddress, "FrameType").ToUpper() == "lfRectangle".ToUpper())
                frametype = tkLabelFrameType.lfRectangle;
            else if (clsreg.ReadDword(strhive, straddress, "FrameType").ToUpper() == "lfRoundedRectangle".ToUpper())
                frametype = tkLabelFrameType.lfRoundedRectangle;

            framecolorleft = clsreg.ReadDword(strhive, straddress, "FrameColorLeft"); //the label frame background color for gradient/left
            if (framecolorleft == "")
                framecolorleft = "DarkGreen";

            framecolorright = clsreg.ReadDword(strhive, straddress, "FrameColorRight");//the label frame background color for gradient/right
            if (framecolorright == "")
                framecolorright = "PaleGreen";

            if (Boolean.TryParse(clsreg.ReadDword(strhive, straddress, "RemoveDuplicates"), out rdupli))//the control to remove duplicate labels 
                removeduplicates = rdupli;
            else
                removeduplicates = false;

            if (Boolean.TryParse(clsreg.ReadDword(strhive, straddress, "AvoidCollision"), out acollision))//the control to avoid label collision
                avoidcollision = acollision;
            else
                avoidcollision = false;

            if (Boolean.TryParse(clsreg.ReadDword(strhive, straddress, "DynamicVisible"), out dvisible))//the control of the label visibility at a given scale
                dynamicvisible = dvisible;
            else
                dynamicvisible = false;

            double maxval;
            double minval;
            if (Double.TryParse(clsreg.ReadDword(strhive, straddress, "MaximumVisibleScale"), out maxval))//the control of the label visibility maximum scale
                maxvisiblescale = maxval;
            else
                maxvisiblescale = 0.0;

            if (Double.TryParse(clsreg.ReadDword(strhive, straddress, "MinimumVisibleScale"), out minval))//the control of the label visibility minimum scale
                minvisiblescale = minval;
            else
                minvisiblescale = 0.0;

            switch (clsreg.ReadDword(strhive, straddress, "LabelAlignment"))
            {
                case ("laBottomCenter"):
                    {
                        labelalignment = tkLabelAlignment.laBottomCenter;
                        break;
                    }
                case ("laBottomLeft"):
                    {
                        labelalignment = tkLabelAlignment.laBottomLeft;
                        break;
                    }
                case ("laBottomRight"):
                    {
                        labelalignment = tkLabelAlignment.laBottomRight;
                        break;
                    }
                case ("laCenter"):
                    {
                        labelalignment = tkLabelAlignment.laCenter;
                        break;
                    }
                case ("laCenterLeft"):
                    {
                        labelalignment = tkLabelAlignment.laCenterLeft;
                        break;
                    }
                case ("laCenterRight"):
                    {
                        labelalignment = tkLabelAlignment.laCenterRight;
                        break;
                    }
                case ("laTopCenter"):
                    {
                        labelalignment = tkLabelAlignment.laTopCenter;
                        break;
                    }
                case ("laTopLeft"):
                    {
                        labelalignment = tkLabelAlignment.laTopLeft;
                        break;
                    }
                case ("laTopRight"):
                    {
                        labelalignment = tkLabelAlignment.laTopRight;
                        break;
                    }
            }

            resultsf = setlabelfinal(sf, labelfield, labelfont, fontcolor, intfontsize, framevisible, frametype, framecolorleft, framecolorright, removeduplicates,
                avoidcollision, shadowvisible, halovisible, dynamicvisible, labelalignment, labelvisible, maxvisiblescale, minvisiblescale);

            return resultsf;
            //int intLbl = -1;
            //int fldIndex = sf.Table.get_FieldIndexByName(labelfield);
            //if (sf.ShapefileType == ShpfileType.SHP_POLYGON)
            //    intLbl = sf.GenerateLabels(fldIndex, MapWinGIS.tkLabelPositioning.lpInteriorPoint, true);
            //else if (sf.ShapefileType == ShpfileType.SHP_POINT)
            //    intLbl = sf.GenerateLabels(fldIndex, tkLabelPositioning.lpCentroid, false);
            //else
            //    intLbl = sf.GenerateLabels(fldIndex, tkLabelPositioning.lpLongestSegement, false);
            ////generate labels (name field is expected in attribute table)

            //sf.Labels.Synchronized = true;
            //String strlayernm = String.Empty;

            //for (int i = 0; i < frmM.legend1.Groups[0].LayerCount; i++)
            //{
            //    if (frmM.legend1.Layers.ItemByHandle(frmM.legend1.Groups[0].LayerHandle(i)).Type != eLayerType.Image)
            //    {
            //        Shapefile sf1 = (Shapefile)frmM.axMap.get_GetObject(frmM.legend1.Groups[0].LayerHandle(i));
            //        if (sf.Filename.ToUpper() == sf1.Filename.ToUpper())
            //        {
            //            strlayernm = frmM.legend1.Map.get_LayerName(i);
            //            break;
            //        }
            //    }
            //}

            //if (intLbl <= 0)
            //{
            //    MessageBox.Show("No labels were generated, \n\rplease check the " + strlayernm + " layer field.", "Labelling Tool");
            //    return resultsf;
            //}

            //Labels lbl = sf.Labels;

            ////setting font
            //lbl.FontColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.FromName(fontcolor)));
            //lbl.FontName = labelfont;
            //lbl.FontSize = intfontsize;
            //lbl.FontOutlineVisible = false;
            //lbl.ShadowColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Yellow));
            //lbl.ShadowVisible = false;
            //lbl.HaloColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Yellow));
            //lbl.HaloVisible = false;
            //lbl.HaloSize = 0;
            ////lbl.TextRenderingHint = tkTextRenderingHint.ClearTypeGridFit;

            //if (shadowvisible == true)
            //{
            //    lbl.ShadowColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray));
            //    lbl.ShadowVisible = true;
            //    lbl.ShadowOffsetX = 1;
            //    lbl.ShadowOffsetY = 1;
            //}
            //lbl.ShadowVisible = shadowvisible;
            //if (halovisible == true)
            //{
            //    lbl.HaloColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray));
            //    lbl.HaloVisible = true;
            //    lbl.HaloSize = 7;
            //}
            //lbl.HaloVisible = halovisible;

            //if (framevisible == true)
            //{
            //    //setting frame
            //    lbl.FrameVisible = framevisible;
            //    lbl.FrameType = frametype;
            //    lbl.FramePaddingY = 10;
            //    lbl.FramePaddingX = 10;
            //    lbl.FrameOutlineStyle = tkDashStyle.dsSolid;
            //    lbl.FrameOutlineColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray));
            //    lbl.FrameGradientMode = tkLinearGradientMode.gmHorizontal;
            //    lbl.FrameBackColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.FromName(framecolorleft)));
            //    lbl.FrameBackColor2 = Convert.ToUInt32(ColorTranslator.ToOle(Color.FromName(framecolorright)));
            //    lbl.FrameOutlineWidth = 1;
            //}
            //else
            //    lbl.FrameVisible = framevisible;

            ////positioning
            //lbl.VerticalPosition = tkVerticalPosition.vpAboveAllLayers;
            ////if (sf.ShapefileType != ShpfileType.SHP_POINT)
            ////    lbl.Alignment = tkLabelAlignment.laCenter;
            ////else if (sf.ShapefileType != ShpfileType.SHP_POLYLINE)
            ////    lbl.Alignment = tkLabelAlignment.laCenter;
            ////else if (sf.ShapefileType != ShpfileType.SHP_POLYGON)
            ////    lbl.Alignment = tkLabelAlignment.laTopCenter;
            //switch (labelalignment)
            //{
            //    case ("laBottomCenter"):
            //        {
            //            lbl.Alignment = tkLabelAlignment.laBottomCenter;
            //            break;
            //        }
            //    case ("laBottomLeft"):
            //        {
            //            lbl.Alignment = tkLabelAlignment.laBottomLeft;
            //            break;
            //        }
            //    case ("laBottomRight"):
            //        {
            //            lbl.Alignment = tkLabelAlignment.laBottomRight;
            //            break;
            //        }
            //    case ("laCenter"):
            //        {
            //            lbl.Alignment = tkLabelAlignment.laCenter;
            //            break;
            //        }
            //    case ("laCenterLeft"):
            //        {
            //            lbl.Alignment = tkLabelAlignment.laCenterLeft;
            //            break;
            //        }
            //    case ("laCenterRight"):
            //        {
            //            lbl.Alignment = tkLabelAlignment.laCenterRight;
            //            break;
            //        }
            //    case ("laTopCenter"):
            //        {
            //            lbl.Alignment = tkLabelAlignment.laTopCenter;
            //            break;
            //        }
            //    case ("laTopLeft"):
            //        {
            //            lbl.Alignment = tkLabelAlignment.laTopLeft;
            //            break;
            //        }
            //    case ("laTopRight"):
            //        {
            //            lbl.Alignment = tkLabelAlignment.laTopRight;
            //            break;
            //        }
            //}

            //lbl.RemoveDuplicates = removeduplicates;
            //lbl.AvoidCollisions = avoidcollision;

            //if (sf.ShapefileType == ShpfileType.SHP_POLYLINE)
            //    lbl.LineOrientation = tkLineLabelOrientation.lorParallel;
            //else
            //    lbl.LineOrientation = tkLineLabelOrientation.lorHorizontal;

            //if (dynamicvisible == true)
            //{
            //    lbl.MaxVisibleScale = 10000;
            //    lbl.MinVisibleScale = 0.0001;
            //}
            //lbl.DynamicVisibility = dynamicvisible;

            //lbl.UseGdiPlus = false;
            //if (labelvisible)
            //    lbl.Visible = true;
            //else
            //    lbl.Visible = false;

            //for (int i = 0; i < sf.Labels.Count; i++)
            //{
            //    if (lbl.get_Label(i, 0).Text.Contains("|") == true)
            //    {
            //        MapWinGIS.Label lb = lbl.get_Label(i, 0);
            //        string val = lb.Text;
            //        string[] arr = val.Split('|');

            //        StringBuilder sb = new StringBuilder();
            //        foreach (string ar in arr)
            //        {
            //            string newval = ar.Replace(".", "");
            //            if (newval != "")
            //            {
            //                sb.Append(ar);
            //                sb.Append(Environment.NewLine);
            //            }
            //        }
            //        lb.Text = sb.ToString();
            //    }
            //}

            
        }


        public Shapefile setlabelfinal(Shapefile sf, string labelfield, string labelfont, string fontcolor, int intfontsize, bool framevisible, tkLabelFrameType frametype, string framecolorleft, string framecolorright, bool removeduplicates, bool avoidcollision, bool shadowvisible, bool halovisible, bool dynamicvisible, tkLabelAlignment labelalignment, bool labelvisible, double maxvisiblescale, double minvisiblescale)
        {
            Shapefile resultsf = sf;
            int intLbl = -1;
            int fldIndex = sf.Table.get_FieldIndexByName(labelfield);
            if (sf.ShapefileType == ShpfileType.SHP_POLYGON)
                intLbl = sf.GenerateLabels(fldIndex, MapWinGIS.tkLabelPositioning.lpInteriorPoint, true);
            else if (sf.ShapefileType == ShpfileType.SHP_POINT)
                intLbl = sf.GenerateLabels(fldIndex, tkLabelPositioning.lpCentroid, false);
            else
                intLbl = sf.GenerateLabels(fldIndex, tkLabelPositioning.lpLongestSegement, false);
            //generate labels (name field is expected in attribute table)

            sf.Labels.Synchronized = true;
            String strlayernm = String.Empty;

            for (int i = 0; i < frmM.legend1.Groups[frmM.m_groupvector].LayerCount; i++)
            {
                if (frmM.legend1.Layers.ItemByHandle(frmM.legend1.Groups[frmM.m_groupvector].LayerHandle(i)).Type != eLayerType.Image)
                {
                    Shapefile sf1 = (Shapefile)frmM.axMap.get_GetObject(frmM.legend1.Groups[frmM.m_groupvector].LayerHandle(i));
                    if (sf.Filename.ToUpper() == sf1.Filename.ToUpper())
                    {
                        strlayernm = frmM.legend1.Map.get_LayerName(i);
                        break;
                    }
                }
            }

            if (intLbl <= 0)
            {
                MessageBox.Show("No labels were generated, \n\rplease check the " + strlayernm + " layer field.", "Labelling Tool");
                return resultsf;
            }

            Labels lbl = sf.Labels;

            //setting font
            lbl.FontColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.FromName(fontcolor)));
            lbl.FontName = labelfont;
            lbl.FontSize = intfontsize;
            lbl.FontOutlineVisible = false;
            lbl.ShadowColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Yellow));
            lbl.ShadowVisible = false;
            lbl.HaloColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Yellow));
            lbl.HaloVisible = false;
            lbl.HaloSize = 0;
            //lbl.TextRenderingHint = tkTextRenderingHint.ClearTypeGridFit;

            if (shadowvisible == true)
            {
                lbl.ShadowColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray));
                lbl.ShadowVisible = shadowvisible;
                lbl.ShadowOffsetX = 1;
                lbl.ShadowOffsetY = 1;
            }
            lbl.ShadowVisible = shadowvisible;

            if (halovisible == true)
            {
                lbl.HaloColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray));
                lbl.HaloVisible = halovisible;
                lbl.HaloSize = 7;
            }
            lbl.HaloVisible = halovisible;

            if (framevisible == true)
            {
                //setting frame
                lbl.FrameVisible = framevisible;
                lbl.FrameType = frametype;
                lbl.FramePaddingY = 10;
                lbl.FramePaddingX = 10;
                lbl.FrameOutlineStyle = tkDashStyle.dsSolid;
                lbl.FrameOutlineColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.Gray));
                lbl.FrameGradientMode = tkLinearGradientMode.gmHorizontal;
                lbl.FrameBackColor = Convert.ToUInt32(ColorTranslator.ToOle(Color.FromName(framecolorleft)));
                lbl.FrameBackColor2 = Convert.ToUInt32(ColorTranslator.ToOle(Color.FromName(framecolorright)));
                lbl.FrameOutlineWidth = 1;
            }
            lbl.FrameVisible = framevisible;

            //positioning
            lbl.VerticalPosition = tkVerticalPosition.vpAboveAllLayers;
            lbl.Alignment = labelalignment;
            //switch (labelalignment)
            //{
            //    case ("laBottomCenter"):
            //        {
            //            lbl.Alignment = tkLabelAlignment.laBottomCenter;
            //            break;
            //        }
            //    case ("laBottomLeft"):
            //        {
            //            lbl.Alignment = tkLabelAlignment.laBottomLeft;
            //            break;
            //        }
            //    case ("laBottomRight"):
            //        {
            //            lbl.Alignment = tkLabelAlignment.laBottomRight;
            //            break;
            //        }
            //    case ("laCenter"):
            //        {
            //            lbl.Alignment = tkLabelAlignment.laCenter;
            //            break;
            //        }
            //    case ("laCenterLeft"):
            //        {
            //            lbl.Alignment = tkLabelAlignment.laCenterLeft;
            //            break;
            //        }
            //    case ("laCenterRight"):
            //        {
            //            lbl.Alignment = tkLabelAlignment.laCenterRight;
            //            break;
            //        }
            //    case ("laTopCenter"):
            //        {
            //            lbl.Alignment = tkLabelAlignment.laTopCenter;
            //            break;
            //        }
            //    case ("laTopLeft"):
            //        {
            //            lbl.Alignment = tkLabelAlignment.laTopLeft;
            //            break;
            //        }
            //    case ("laTopRight"):
            //        {
            //            lbl.Alignment = tkLabelAlignment.laTopRight;
            //            break;
            //        }
            //}

            lbl.RemoveDuplicates = removeduplicates;
            lbl.AvoidCollisions = avoidcollision;

            if (sf.ShapefileType == ShpfileType.SHP_POLYLINE)
                lbl.LineOrientation = tkLineLabelOrientation.lorParallel;
            else
                lbl.LineOrientation = tkLineLabelOrientation.lorHorizontal;

            //if (dynamicvisible == true)
            //{
            lbl.MaxVisibleScale = maxvisiblescale;//10000;
            lbl.MinVisibleScale = minvisiblescale;//0.0001;
            //}
            //lbl.DynamicVisibility = true;
            lbl.DynamicVisibility = dynamicvisible;

            lbl.UseGdiPlus = false;
            if (labelvisible)
                lbl.Visible = true;
            else
                lbl.Visible = false;

            for (int i = 0; i < sf.Labels.Count; i++)
            {
                if (lbl.get_Label(i, 0).Text.Contains("|") == true)
                {
                    MapWinGIS.Label lb = lbl.get_Label(i, 0);
                    string val = lb.Text;
                    string[] arr = val.Split('|');

                    StringBuilder sb = new StringBuilder();
                    foreach (string ar in arr)
                    {
                        string newval = ar.Replace(".", "");
                        if (newval != "")
                        {
                            sb.Append(ar);
                            sb.Append(Environment.NewLine);
                        }
                    }
                    lb.Text = sb.ToString();
                }
            }

            return resultsf;

        }

        public void savetokml(Shapefile sf, string strlayernm, string strfilenm)
        {
            System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (sf == null)
                    return;

                DataTable dt = new DataTable();
                if (sf.ShapefileType == ShpfileType.SHP_POINT || sf.ShapefileType == ShpfileType.SHP_POLYGON || sf.ShapefileType == ShpfileType.SHP_POLYLINE)
                    dt = populateDataTable(sf);

                geDocument kmlDocument = new geDocument();
                kmlDocument.Name = System.IO.Path.GetFileName(strfilenm);
                //create the kml folder
                geFolder folder = new geFolder();
                folder.Name = "kml";
                folder.Open = true;
                folder.Description = "kml files";

                foreach (DataRow row in dt.Rows)
                {
                    int intid = -1;
                    int shpid = -1;
                    if (int.TryParse(row[0].ToString(), out intid))
                        shpid = intid;

                    StringBuilder sbkml = new StringBuilder();
                    sbkml.Append("<html>");
                    sbkml.Append("<body>");
                    sbkml.Append("<form>");
                    sbkml.Append("<table border='1' width='350px'>");
                    //// kml title -- layer name
                    sbkml.AppendFormat("<tr style='border-color:white;'><td style='background-color:#DADAFF;text-align:center;text-shadow: 0 0 0.2em white, 0 0 0.2em white,0 0 0.2em white;' colspan='2'><font face='Arial' size='4px' style='font-style:regular;color:orange;'><strong>{0}</strong></font></td></tr>", strlayernm.ToUpper());
                    foreach (DataColumn column in dt.Columns)
                    {
                        ////field column -- left side of the kml description
                        ////record column -- right side of the kml description
                        sbkml.AppendFormat("<tr style='border-color:white;'><td width='40%' style='background-color:#8D8DDA;text-shadow: 0 0 0.2em white, 0 0 0.2em white,0 0 0.2em white;'><font face='Arial' size='3px' style='font-style:regular;color:white;'><strong>{0}</strong></font></td><td width='60%' style='background-color:#E9E9E9;'><font face='Arial' size='3px' style='font-style:italic;color:#000000;'>{1}</font></td></tr>", column.ColumnName.ToUpper(), row[column].ToString());
                    }
                    sbkml.Append("</table>");
                    sbkml.Append("</form>");
                    sbkml.Append("</body>");
                    sbkml.Append("</html>");

                    string strDesc = sbkml.ToString();
                    MapWinGIS.Shape sh = sf.get_Shape(shpid);

                    ShapefileCategory shpcat = sf.Categories.get_Item(sf.get_ShapeCategory(shpid));
                    MapWinGIS.ShapeDrawingOptions options;
                    if (shpcat == null)
                        options = sf.DefaultDrawingOptions;
                    else
                        options = sf.Categories.get_Item(sf.get_ShapeCategory(shpid)).DrawingOptions;

                    if (sh != null && options != null && strDesc != null)
                        processPoly(folder, sh, "", strDesc, options);
                }
                kmlDocument.Features.Add(folder);
                //creating the kml
                geKML kml = new geKML(kmlDocument);
                //writing the kml
                File.WriteAllBytes(strfilenm, kml.ToKML());
                string strkmlfilecontent = File.ReadAllText(strfilenm);

                Process kmlProcess = new Process();
                clsRegistry clsreg = new clsRegistry();
                String GEPath = clsreg.ReadDword("CurrentUser", @"Software\Google\Google Earth Plus", "InstallLocation") + @"client/googleearth.exe";
                if (GEPath == null || GEPath == "" || GEPath == @"/googleearth.exe")
                {
                    System.Windows.Forms.MessageBox.Show("There are no google earth installed on this machine.", "Export Drawn Parcels", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    System.Windows.Forms.Cursor.Current = Cursors.Default;
                    return;
                }

                kmlProcess.StartInfo.FileName = GEPath;//@"R:\personal\Project\GoogleEarthViewer\GoogleEarthViewer\bin\Debug\GoogleEarthViewer.exe";//
                kmlProcess.StartInfo.Arguments = "\"" + (String)strfilenm + "\"";
                kmlProcess.Start();

                System.Windows.Forms.Cursor.Current = Cursors.Default;
            }
            catch
            {
                System.Windows.Forms.Cursor.Current = Cursors.Default;
            }
        }

        public void processPoly(geFolder folder, MapWinGIS.Shape sh, String strPin, String strDesc, ShapeDrawingOptions options)
        {
            double coordLat;
            double coordLong;
            MapWinGIS.Point polyVertex;

            #region add points to polyline or polygon
            //create a placemark for the line
            gePlacemark pmPolygon = new gePlacemark();
            pmPolygon.StyleUrl = "";//"#Shape2KMLGeneratedStyle";
            pmPolygon.Name = "";//"PIN: " + strPin ;
            pmPolygon.Description = strDesc;
            pmPolygon.Visibility = true;

            #region Create kml style

            geStyle style = new geStyle("");
            //style.ID = "Shape2KMLGeneratedStyle";
            float linewidth = (float)2;
            float ptsize = (float)2;
            Color linecolor = Color.White;
            Color fillcolor = Color.FromArgb(150, Color.YellowGreen);
            Color ptcolor = Color.Red;
            bool isfill = false;
            bool isoutline = true;
            float fillopacity = (float)255;
            float lineopacity = (float)255;
            float ptopacity = (float)255;

            if (options != null)
            {
                if (sh.ShapeType == ShpfileType.SHP_POLYLINE)
                {
                    linewidth = options.LineWidth;
                    linecolor = MapWinUtility.Colors.IntegerToColor(options.LineColor);
                    isoutline = options.LineVisible;
                    lineopacity = options.LineTransparency;
                }
                else if (sh.ShapeType == ShpfileType.SHP_POLYGON)
                {
                    linewidth = options.LineWidth;
                    linecolor = MapWinUtility.Colors.IntegerToColor(options.LineColor);
                    isoutline = options.LineVisible;
                    lineopacity = options.LineTransparency;
                    fillcolor = MapWinUtility.Colors.IntegerToColor(options.FillColor);
                    isfill = options.FillVisible;
                    fillopacity = options.FillTransparency;

                }
                else if (sh.ShapeType == ShpfileType.SHP_POINT)
                {
                    ptsize = options.PointSize;
                    ptopacity = options.FillTransparency;
                    ptcolor = MapWinUtility.Colors.IntegerToColor(options.FillColor);
                }
            }


            if (sh.ShapeType == ShpfileType.SHP_POLYLINE)
            {
                style.LineStyle = new geLineStyle();
                style.LineStyle.Color.SysColor = Color.FromArgb((Int32)lineopacity, linecolor);// Color.White;
                style.LineStyle.Width = linewidth;// (float)2;
            }
            else if (sh.ShapeType == ShpfileType.SHP_POLYGON)
            {
                style.LineStyle = new geLineStyle();
                style.LineStyle.Color.SysColor = Color.FromArgb((Int32)lineopacity, linecolor);// Color.White;
                style.LineStyle.Width = linewidth;// (float)2;

                style.PolyStyle = new gePolyStyle();
                style.PolyStyle.Color.SysColor = Color.FromArgb((Int32)fillopacity, fillcolor);
                style.PolyStyle.ColorMode = geColorModeEnum.normal;
                style.PolyStyle.Fill = isfill;// false;
                style.PolyStyle.Outline = isoutline;// true;

            }
            else if (sh.ShapeType == ShpfileType.SHP_POINT)
            {
                style.IconStyle = new geIconStyle();
                style.IconStyle.Scale = (float)1;//ptsize;
                style.IconStyle.Color.SysColor = Color.FromArgb((Int32)ptopacity, ptcolor);
                style.IconStyle.HotSpot.x = 32.0;
                style.IconStyle.HotSpot.y = 32.0;
                style.IconStyle.HotSpot.xunits = geUnitsEnum.pixels;
                style.IconStyle.HotSpot.yunits = geUnitsEnum.pixels;
                //MapWinGIS.Image img;
                //if (options.Picture != null)
                //{
                //    img = options.Picture;
                //    style.IconStyle.Icon = new geIcon(img.Filename);
                //}
            }

            #endregion

            pmPolygon.StyleSelectors.Add(style);
            List<geCoordinates> polyCoords = new List<geCoordinates>();
            geCoordinates gecoord = new geCoordinates(new geAngle90(0), new geAngle180(0));
            if (sh.ShapeType == ShpfileType.SHP_POLYLINE || sh.ShapeType == ShpfileType.SHP_POLYGON)
            {
                for (int ii = 0; ii < sh.numPoints; ii++)
                {
                    //add point to line
                    polyVertex = sh.get_Point(ii);
                    coordLat = polyVertex.y;
                    coordLong = polyVertex.x;
                    try
                    {
                        polyCoords.Add(new geCoordinates(new geAngle90(coordLat), new geAngle180(coordLong)));
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Altitude field is not a number value");
                        break;
                    }
                }
            }
            else if (sh.ShapeType == ShpfileType.SHP_POINT)
            {
                polyVertex = sh.get_Point(0);
                coordLat = polyVertex.y;
                coordLong = polyVertex.x;
                gecoord = new geCoordinates(new geAngle90(coordLat), new geAngle180(coordLong));
            }

            if (sh.ShapeType == ShpfileType.SHP_POLYGON)
            {
                //create line from list of coords
                geOuterBoundaryIs outer = new geOuterBoundaryIs(new geLinearRing(polyCoords));
                gePolygon poly = new gePolygon(outer);
                poly.Extrude = true;
                poly.Tessellate = true;
                poly.AltitudeMode = geAltitudeModeEnum.clampToGround;
                pmPolygon.Geometry = poly;
            }
            else if (sh.ShapeType == ShpfileType.SHP_POLYLINE)
            {
                geLineString geline = new geLineString(polyCoords);
                geline.Extrude = true;
                geline.Tessellate = true;
                geline.AltitudeMode = geAltitudeModeEnum.clampToGround;
                pmPolygon.Geometry = geline;
            }
            else if (sh.ShapeType == ShpfileType.SHP_POINT)
            {
                gePoint gept = new gePoint(gecoord);
                gept.Extrude = true;
                gept.Tessellate = true;
                gept.AltitudeMode = geAltitudeModeEnum.clampToGround;
                pmPolygon.Geometry = gept;
            }

            if (pmPolygon != null)
                folder.Features.Add(pmPolygon);
            #endregion
        }

        public Shapefile legendPointDefault(Shapefile sf, UInt32 trnsColor, UInt32 trnsColor2, bool useTrans, double picXscale, double picYscale)
        {
            try
            {
                Utils myUtil = new Utils();
                sf.DefaultDrawingOptions.PointShape = tkPointShapeType.ptShapeCircle;
                sf.DefaultDrawingOptions.PictureScaleX = picXscale;//1
                sf.DefaultDrawingOptions.PictureScaleY = picYscale;//1
                sf.CollisionMode = tkCollisionMode.AllowCollisions;
                sf.DefaultDrawingOptions.PointRotation = (double)0;
                sf.DefaultDrawingOptions.PointSidesRatio = (float)0.5;

                return sf;
            }
            catch
            {
                sf = null;
                return sf;
            }
        }

        public void setflddefaultval(Shapefile sf, string strfld)
        {
            int fldidx = -1;
            if ((sf.EditingShapes == true) && (sf.EditingTable == true))
            {
                fldidx = sf.Table.get_FieldIndexByName(strfld);
                if (fldidx > -1)
                {
                    FieldType fldtype = sf.Table.get_Field(fldidx).Type;
                    for (int i = 0; i < sf.NumShapes; i++)
                    {
                        if(fldtype == FieldType.STRING_FIELD)
                            sf.EditCellValue(fldidx, i, "");
                        else if(fldtype == FieldType.INTEGER_FIELD)
                            sf.EditCellValue(fldidx, i, 0);
                        else if(fldtype == FieldType.DOUBLE_FIELD)
                            sf.EditCellValue(fldidx, i, 0.0);
                    }
                }
            }
        }

        public Shapefile ConvertTextToShapeFile(String filenm, DataTable dt, String xfld, String yfld)
        {
            Shapefile sf = new MapWinGIS.Shapefile();
            try
            {
                string sfname = String.Empty;

                if (System.IO.File.Exists(Path.GetFileNameWithoutExtension(filenm) + ".shp"))
                    System.IO.File.Delete(Path.GetFileNameWithoutExtension(filenm) + ".shp");
                if (System.IO.File.Exists(Path.GetFileNameWithoutExtension(filenm) + ".shx"))
                    System.IO.File.Delete(Path.GetFileNameWithoutExtension(filenm) + ".shx");
                if (System.IO.File.Exists(Path.GetFileNameWithoutExtension(filenm) + ".prj"))
                    System.IO.File.Delete(Path.GetFileNameWithoutExtension(filenm) + ".prj");
                if (System.IO.File.Exists(Path.GetFileNameWithoutExtension(filenm) + ".dbf"))
                    System.IO.File.Delete(Path.GetFileNameWithoutExtension(filenm) + ".dbf");

                bool result = sf.CreateNew(Path.GetFileNameWithoutExtension(filenm) + ".shp", MapWinGIS.ShpfileType.SHP_POINT);

                int intfld = 0;
                if (result == true)
                    result = sf.StartEditingShapes(true, null);
                
                MapWinGIS.Field fld = new MapWinGIS.FieldClass();
                for (int i = 0; i < dt.Columns.Count; i++ )
                {
                    if (i == 0)
                        fld = new MapWinGIS.FieldClass();
                    else
                    {
                        intfld += 1;
                        fld = new MapWinGIS.FieldClass();
                    }

                    fld.Name = dt.Columns[i].ColumnName.ToString();

                    double dblval;
                    bool booldbl = Double.TryParse(dt.Rows[1][dt.Columns[i]].ToString(), out dblval);//Double.TryParse(dt.Rows[1].Field<string>(dt.Columns[i].ColumnName).ToString(), out dblval);
                    if (booldbl)
                        fld.Type = MapWinGIS.FieldType.DOUBLE_FIELD;
                    else
                    {
                        int intval;
                        bool boolint = Int32.TryParse(dt.Rows[1][dt.Columns[i]].ToString(), out intval);
                        if (boolint)
                            fld.Type = MapWinGIS.FieldType.INTEGER_FIELD;
                        else
                        {
                            string boolstr = dt.Rows[1][dt.Columns[i]].ToString();
                            if (boolstr != "")
                                fld.Type = MapWinGIS.FieldType.STRING_FIELD;
                        }
                    }
                    if (dt.Columns[i].MaxLength != -1)
                        fld.Width = dt.Columns[i].MaxLength;
                    else
                        fld.Width = 225;

                    sf.EditInsertField(fld, ref intfld, null);
                }

                int ptref = 0;
                bool boolSuccess = false;

                for (int irow = 0; irow < dt.Rows.Count; irow++)
                {
                    double dblx = 0.0;
                    double dbly = 0.0;
                    MapWinGIS.Point shpt = new MapWinGIS.Point();
                    bool boolx = Double.TryParse(dt.Rows[irow][xfld].ToString(), out dblx);//Double.TryParse(dt.Rows[irow].Field<String>(xfld).ToString(), out dblx);
                    if(boolx)
                        shpt.x = dblx;
                    bool booly = Double.TryParse(dt.Rows[irow][yfld].ToString(), out dbly);//Double.TryParse(dt.Rows[irow].Field<String>(yfld).ToString(), out dbly);
                    if (boolx)
                        shpt.y = dbly;

                    if (dblx > 0 && dbly > 0)
                    {
                        MapWinGIS.Shape sh1 = new MapWinGIS.ShapeClass();
                        boolSuccess = sh1.Create(MapWinGIS.ShpfileType.SHP_POINT);
                        ptref += 1;

                        boolSuccess = sh1.InsertPoint(shpt, ref ptref);
                        boolSuccess = sf.EditInsertShape(sh1, ref ptref);

                        bool boolCellAdd;
                        for (int icol = 0; icol < dt.Columns.Count; icol++)
                        {
                            boolCellAdd = sf.EditCellValue(icol, ptref, dt.Rows[irow][icol]);
                        }
                    }
                }

                boolSuccess = sf.StopEditingShapes(true, true, null);
                sf.SaveAs(filenm, null);

                return sf;
            }
            catch
            {
                return null;
            }

        }

        public Shapefile JoinTolayer(String filenm, DataTable layerdt, DataTable sourcedt, String layerfld, String sourcefld, Shapefile m_sf)
        {
            Shapefile sf = new MapWinGIS.Shapefile();
            try
            {
                string sfname = String.Empty;

                if (System.IO.File.Exists(Path.GetFileNameWithoutExtension(filenm) + ".shp"))
                    System.IO.File.Delete(Path.GetFileNameWithoutExtension(filenm) + ".shp");
                if (System.IO.File.Exists(Path.GetFileNameWithoutExtension(filenm) + ".shx"))
                    System.IO.File.Delete(Path.GetFileNameWithoutExtension(filenm) + ".shx");
                if (System.IO.File.Exists(Path.GetFileNameWithoutExtension(filenm) + ".prj"))
                    System.IO.File.Delete(Path.GetFileNameWithoutExtension(filenm) + ".prj");
                if (System.IO.File.Exists(Path.GetFileNameWithoutExtension(filenm) + ".dbf"))
                    System.IO.File.Delete(Path.GetFileNameWithoutExtension(filenm) + ".dbf");

                bool result = sf.CreateNew(Path.GetFileNameWithoutExtension(filenm) + ".shp",m_sf.ShapefileType);

                int intfld = 0;
                if (result == true)
                    result = sf.StartEditingShapes(true, null);

                //start creating fields from layer fields
                MapWinGIS.Field fld = new MapWinGIS.FieldClass();
                for (int i = 0; i < layerdt.Columns.Count; i++)
                {
                    if (i == 0)
                        fld = new MapWinGIS.FieldClass();
                    else
                    {
                        intfld += 1;
                        fld = new MapWinGIS.FieldClass();
                    }

                    fld.Name = layerdt.Columns[i].ColumnName.ToString();

                    double dblval;
                    bool booldbl = Double.TryParse(layerdt.Rows[1][i].ToString(), out dblval);
                    if (booldbl)
                        fld.Type = MapWinGIS.FieldType.DOUBLE_FIELD;
                    else
                    {
                        int intval;
                        bool boolint = Int32.TryParse(layerdt.Rows[1][i].ToString(), out intval);//Field<string>(layerdt.Columns[i].ColumnName)
                        if (boolint)
                            fld.Type = MapWinGIS.FieldType.INTEGER_FIELD;
                        else
                        {
                            string boolstr = layerdt.Rows[1][i].ToString();//.Field<string>(layerdt.Columns[i].ColumnName
                            if (boolstr != "")
                                fld.Type = MapWinGIS.FieldType.STRING_FIELD;
                        }
                    }
                    if (layerdt.Columns[i].MaxLength != -1)
                        fld.Width = layerdt.Columns[i].MaxLength;
                    else
                        fld.Width = 225;

                    sf.EditInsertField(fld, ref intfld, null);
                }
                for (int ii = 0; ii < sourcedt.Columns.Count; ii++)
                {
                    if (sourcedt.Columns[ii].ColumnName.ToString() != sourcefld)
                    {
                        intfld += 1;
                        fld = new MapWinGIS.FieldClass();
                        fld.Name = sourcedt.Columns[ii].ColumnName.ToString();
                        double dblval;
                        bool booldbl = Double.TryParse(sourcedt.Rows[1][ii].ToString(), out dblval);//.Field<string>(sourcedt.Columns[ii].ColumnName)
                        if (booldbl)
                            fld.Type = MapWinGIS.FieldType.DOUBLE_FIELD;
                        else
                        {
                            int intval;
                            bool boolint = Int32.TryParse(sourcedt.Rows[1][ii].ToString(), out intval);//.Field<string>(sourcedt.Columns[ii].ColumnName)
                            if (boolint)
                                fld.Type = MapWinGIS.FieldType.INTEGER_FIELD;
                            else
                            {
                                string boolstr = sourcedt.Rows[1][ii].ToString();//.Field<string>(sourcedt.Columns[ii].ColumnName)
                                if (boolstr != "")
                                    fld.Type = MapWinGIS.FieldType.STRING_FIELD;
                            }
                        }
                        if (sourcedt.Columns[ii].MaxLength != -1)
                            fld.Width = sourcedt.Columns[ii].MaxLength;
                        else
                            fld.Width = 225;

                        sf.EditInsertField(fld, ref intfld, null);
                    }
                }
                int ptref = 0;
                bool boolSuccess = false;
                ////get the records for the shapefile

                if (sf.EditingShapes == false)
                    sf.StartEditingShapes(true, null);
                if (sf.EditingTable == false)
                    sf.StartEditingTable(null);

                for (int iii = 0; iii < m_sf.NumShapes; iii++)
                {
                    //Set the text for this shape
                    int ilayeridx = m_sf.Table.get_FieldIndexByName(layerfld);
                    string strlayerval = m_sf.get_CellValue(ilayeridx, iii).ToString();
                    MapWinGIS.Shape sh = m_sf.get_Shape(iii);

                    if (sf.EditingShapes == false)
                        sf.StartEditingShapes(true, null);
                    if (sf.EditingTable == false)
                        sf.StartEditingTable(null);

                    ptref += 1;
                    boolSuccess = sf.EditInsertShape(sh, ref ptref);

                    bool boolCellAdd;
                    int colcntr = 0;
                    for (int ishpcol = 0; ishpcol < m_sf.Table.NumFields; ishpcol++)
                    {
                        for (int ilayercol = 0; ilayercol < layerdt.Columns.Count; ilayercol++)
                        {
                            if (layerdt.Columns[ilayercol].ColumnName == m_sf.Table.Field[ishpcol].Name)
                            {
                                boolCellAdd = sf.EditCellValue(colcntr, ptref, m_sf.get_CellValue(ishpcol, iii).ToString());
                                colcntr += 1;
                            }
                        }
                    }

                    for (int isourcerow = 0; isourcerow < sourcedt.Rows.Count; isourcerow++)
                    {
                        int idx = sourcedt.Rows[isourcerow].Table.Columns[sourcefld].Ordinal;
                        if(sourcedt.Rows[isourcerow][idx].ToString() == strlayerval)
                        {
                            for (int isourcecol = 0; isourcecol < sourcedt.Columns.Count; isourcecol++)
                            {
                                if (sourcedt.Rows[isourcerow][isourcecol].ToString() != strlayerval)
                                {
                                    boolCellAdd = sf.EditCellValue(colcntr, ptref, sourcedt.Rows[isourcerow][isourcecol].ToString());
                                    colcntr += 1;
                                }
                            }
                        }
                    }
                }

                boolSuccess = sf.StopEditingShapes(true, true, null);
                boolSuccess = sf.StopEditingTable(true, null);
                sf.SaveAs(filenm, null);

                return sf;
            }
            catch
            {
                return null;
            }
        }

        //Private Sub WebMercatorToGeographic(ByVal mercatorX As Double, ByVal mercatorY As Double, ByRef lat As Double, ByRef lon As Double)
        //If ((mercatorX < -20037508.3427892) _
        //OrElse (mercatorX > 20037508.3427892)) Then
        //Throw New ArgumentException("Point does not fall within a valid range of the mercator projection.")
        //End If
        //Dim x As Double = mercatorX
        //Dim y As Double = mercatorY
        //Dim num3 As Double = (x / 6378137)
        //Dim num4 As Double = (num3 * 57.295779513082323)
        //Dim num5 As Double = Math.Floor(CType(((num4 + 180) / 360), Double))
        //Dim num6 As Double = (num4 - (num5 * 360))
        //Dim num7 As Double = (1.5707963267948966 - (2 * Math.Atan(Math.Exp((((1 * y) * -1) / 6378137)))))
        //lat = num6
        //lon = (num7 * 57.295779513082323)
        //End Sub

        //Private Sub GeographicToWebMercator(ByVal lat As Double, ByVal lon As Double, ByRef mercatorX As Double, ByRef mercatorY As Double)
        //If ((lat < -90) _
        //OrElse (lon > 90)) Then
        //Throw New ArgumentException("Point does not fall within a valid range of a geographic coordinate system.")
        //End If
        //Dim num As Double = (lat * 0.017453292519943295)
        //Dim x As Double = (6378137 * num)
        //Dim a As Double = (lon * 0.017453292519943295)
        //mercatorX = x
        //mercatorY = (3189068.5 * Math.Log(((1 + Math.Sin(a)) / (1 - Math.Sin(a)))))
        //End Sub


        /// <summary>
        /// last of the clscommon line
        /// </summary>


    }

    /// <summary>
    /// icon converter
    /// </summary>
    //public class IconConverter : System.Windows.Forms.AxHost
    //{
    //    private IconConverter()
    //        : base(string.Empty)
    //    {
    //    }

    //    //public static stdole.IPictureDisp GetIPictureDispFromImage(System.Drawing.Image image)
    //    //{

    //    //    return (stdole.IPictureDisp)GetIPictureDispFromPicture(image);
    //    //}
    //    public static object GetIPictureDispFromImage(object image)
    //    {
    //        return (object)GetIPictureDispFromImage(image);
    //    }
    //}

    /// <summary>
    /// This class implements the fast CohenSutherland line/rectangle intersect test
    /// We use this class to quickly eliminate line that hang off the screen to increase
    /// drawing speed significantly when lines start off screen!
    /// </summary>
    public static class CohenSutherland
    {
        private static int RIGHT = 2;
        private static int TOP = 8;
        private static int BOTTOM = 4;
        private static int LEFT = 1;

        private static int computeOutCode(int x, int y, int xmin, int ymin, int xmax, int ymax)
        {
            int code = 0;
            if (y > ymax)
                code |= TOP;
            else if (y < ymin)
                code |= BOTTOM;
            if (x > xmax)
                code |= RIGHT;
            else if (x < xmin)
                code |= LEFT;
            return code;
        }

        /// <summary>
        /// Calculates the intersection of a line (pt1,pt2) and a rectangle
        /// It returns a GraphicsPath composed of the line entirely withint rect or null if non exists
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static GraphicsPath cohenSutherland(System.Drawing.Point pt1, System.Drawing.Point pt2, Rectangle rect)
        {
            int x1 = pt1.X;
            int y1 = pt1.Y;
            int x2 = pt2.X;
            int y2 = pt2.Y;
            int xmin = rect.Left;
            int ymin = rect.Top;
            int xmax = rect.Bottom;
            int ymax = rect.Right;

            //Outcodes for P0, P1, and whatever point lies outside the clip rectangle
            int outcode0, outcode1, outcodeOut, hhh = 0;
            bool accept = false, done = false;

            //compute outcodes
            outcode0 = computeOutCode(x1, y1, xmin, ymin, xmax, ymax);
            outcode1 = computeOutCode(x2, y2, xmin, ymin, xmax, ymax);

            do
            {
                if ((outcode0 | outcode1) == 0)
                {
                    accept = true;
                    done = true;
                }
                else if ((outcode0 & outcode1) > 0)
                {
                    done = true;
                }

                else
                {
                    //failed both tests, so calculate the line segment to clip
                    //from an outside point to an intersection with clip edge
                    int x = 0, y = 0;
                    //At least one endpoint is outside the clip rectangle; pick it.
                    outcodeOut = outcode0 != 0 ? outcode0 : outcode1;
                    //Now find the intersection point;
                    //use formulas y = y0 + slope * (x - x0), x = x0 + (1/slope)* (y - y0)
                    if ((outcodeOut & TOP) > 0)
                    {
                        x = x1 + (x2 - x1) * (ymax - y1) / (y2 - y1);
                        y = ymax;
                    }
                    else if ((outcodeOut & BOTTOM) > 0)
                    {
                        x = x1 + (x2 - x1) * (ymin - y1) / (y2 - y1);
                        y = ymin;
                    }
                    else if ((outcodeOut & RIGHT) > 0)
                    {
                        y = y1 + (y2 - y1) * (xmax - x1) / (x2 - x1);
                        x = xmax;
                    }
                    else if ((outcodeOut & LEFT) > 0)
                    {
                        y = y1 + (y2 - y1) * (xmin - x1) / (x2 - x1);
                        x = xmin;
                    }
                    //Now we move outside point to intersection point to clip
                    //and get ready for next pass.
                    if (outcodeOut == outcode0)
                    {
                        x1 = x;
                        y1 = y;
                        outcode0 = computeOutCode(x1, y1, xmin, ymin, xmax, ymax);
                    }
                    else
                    {
                        x2 = x;
                        y2 = y;
                        outcode1 = computeOutCode(x2, y2, xmin, ymin, xmax, ymax);
                    }
                }
                hhh++;
            }
            while (done != true && hhh < 5000);

            if (accept)
            {
                GraphicsPath gp = new GraphicsPath();
                gp.AddLine(x1, y1, x2, y2);
                return (gp);
            }
            return (null);
        }
    }
}