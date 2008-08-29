BULK INSERT [Role]
FROM '$(ScliptPath)INSERT/Role.txt'
WITH(
      DATAFILETYPE = 'widechar'
)
GO

BULK INSERT [Privilege]
FROM '$(ScliptPath)INSERT/Privilege.txt'
WITH(
      DATAFILETYPE = 'widechar'
)
GO

BULK INSERT [Role_Privilege]
FROM '$(ScliptPath)INSERT/Role_Privilege.txt'
WITH(
      DATAFILETYPE = 'widechar'
)
GO

BULK INSERT [PurchasingUser]
FROM '$(ScliptPath)INSERT/PurchasingUser.txt'
WITH(
      DATAFILETYPE = 'widechar'
)
GO

BULK INSERT [PurchasingCountry]
FROM '$(ScliptPath)INSERT/PurchasingCountry.txt'
WITH(
      DATAFILETYPE = 'widechar'
)
GO

BULK INSERT [PurchasingUnit]
FROM '$(ScliptPath)INSERT/PurchasingUnit.txt'
WITH(
      DATAFILETYPE = 'widechar'
)
GO

BULK INSERT [PurchasingCurrency]
FROM '$(ScliptPath)INSERT/PurchasingCurrency.txt'
WITH(
      DATAFILETYPE = 'widechar'
)
GO

BULK INSERT [PurchasingPaymentTerm]
FROM '$(ScliptPath)INSERT/PurchasingPaymentTerm.txt'
WITH(
      DATAFILETYPE = 'widechar'
)
GO

BULK INSERT [Purpose]
FROM '$(ScliptPath)INSERT/Purpose.txt'
WITH(
      DATAFILETYPE = 'widechar'
)
GO

BULK INSERT [NoOfferReason]
FROM '$(ScliptPath)INSERT/NoOfferReason.txt'
WITH(
      DATAFILETYPE = 'widechar'
)
GO

BULK INSERT [RFQCorres]
FROM '$(ScliptPath)INSERT/RFQCorres.txt'
WITH(
      DATAFILETYPE = 'widechar'
)
GO

BULK INSERT [POCorres]
FROM '$(ScliptPath)INSERT/POCorres.txt'
WITH(
      DATAFILETYPE = 'widechar'
)
GO

BULK INSERT [RFQStatus]
FROM '$(ScliptPath)INSERT/RFQStatus.txt'
WITH(
      DATAFILETYPE = 'widechar'
)
GO

BULK INSERT [POStatus]
FROM '$(ScliptPath)INSERT/POStatus.txt'
WITH(
      DATAFILETYPE = 'widechar'
)
GO
