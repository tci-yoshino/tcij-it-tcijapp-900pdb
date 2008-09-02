BULK INSERT [Role]
FROM '$(ScriptPath)INSERT/Role.txt'
WITH(
      DATAFILETYPE = 'widechar'
)
GO

BULK INSERT [Privilege]
FROM '$(ScriptPath)INSERT/Privilege.txt'
WITH(
      DATAFILETYPE = 'widechar'
)
GO

BULK INSERT [Role_Privilege]
FROM '$(ScriptPath)INSERT/Role_Privilege.txt'
WITH(
      DATAFILETYPE = 'widechar'
)
GO

BULK INSERT [PurchasingUser]
FROM '$(ScriptPath)INSERT/PurchasingUser.txt'
WITH(
      DATAFILETYPE = 'widechar'
)
GO

BULK INSERT [PurchasingCountry]
FROM '$(ScriptPath)INSERT/PurchasingCountry.txt'
WITH(
      DATAFILETYPE = 'widechar'
)
GO

BULK INSERT [PurchasingUnit]
FROM '$(ScriptPath)INSERT/PurchasingUnit.txt'
WITH(
      DATAFILETYPE = 'widechar'
)
GO

BULK INSERT [PurchasingCurrency]
FROM '$(ScriptPath)INSERT/PurchasingCurrency.txt'
WITH(
      DATAFILETYPE = 'widechar'
)
GO

BULK INSERT [PurchasingPaymentTerm]
FROM '$(ScriptPath)INSERT/PurchasingPaymentTerm.txt'
WITH(
      DATAFILETYPE = 'widechar'
)
GO

BULK INSERT [Purpose]
FROM '$(ScriptPath)INSERT/Purpose.txt'
WITH(
      DATAFILETYPE = 'widechar'
)
GO

BULK INSERT [NoOfferReason]
FROM '$(ScriptPath)INSERT/NoOfferReason.txt'
WITH(
      DATAFILETYPE = 'widechar'
)
GO

BULK INSERT [RFQCorres]
FROM '$(ScriptPath)INSERT/RFQCorres.txt'
WITH(
      DATAFILETYPE = 'widechar'
)
GO

BULK INSERT [POCorres]
FROM '$(ScriptPath)INSERT/POCorres.txt'
WITH(
      DATAFILETYPE = 'widechar'
)
GO

BULK INSERT [RFQStatus]
FROM '$(ScriptPath)INSERT/RFQStatus.txt'
WITH(
      DATAFILETYPE = 'widechar'
)
GO

BULK INSERT [POStatus]
FROM '$(ScriptPath)INSERT/POStatus.txt'
WITH(
      DATAFILETYPE = 'widechar'
)
GO
