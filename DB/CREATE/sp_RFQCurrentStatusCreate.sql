/****** オブジェクト:  StoredProcedure [dbo].[sp_RFQCurrentStatusCreate]    スクリプト日付: 08/10/2009 14:13:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		OKUDA
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_RFQCurrentStatusCreate]
AS
BEGIN
	SET NOCOUNT ON;
	TRUNCATE TABLE RFQCurrentStatus
	INSERT INTO RFQCurrentStatus SELECT * FROM v_RFQCurrentStatus
END

GO