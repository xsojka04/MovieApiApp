cd webapi 

Misto init jmeno migrace
dotnet ef migrations add Init --project ../Data			--vytvoreni migrace

dotnet ef database update								--aplikace migrace

pro restart jit pres webclienta a pomazat to rucne (pouzivame nejaky zdarma hosting a zlobí to jinak)
