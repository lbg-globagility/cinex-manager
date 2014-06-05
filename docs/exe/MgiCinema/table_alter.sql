
GO

/****** Object:  Table [dbo].[cinemas]    Script Date: 12/19/2012 10:32:31 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

ALTER TABLE [dbo].[cinemas]
ADD	[PR_number] [varchar](50) NULL
 


ALTER TABLE [dbo].[system_parameters]
ADD	[accreditation_no] [varchar](50) NULL,
	[TIN] [varchar](50) NULL
	
