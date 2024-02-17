# CEVA manažment nakladania a expedície vlakov 
Projekt TIS 2023 o nakladaní vozňov vlaku 

## Predpoklady 
Releas z https://github.com/TIS2023-FMFI/vlaky alebo  
neklonovať repozitár a skompilovať podľa https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/?view=aspnetcore-8.0&source=recommendations#publish-to-a-folder 

Ďalšie: 
- PostgreSQL 
- Apache 
- Aspnetcore-runtime-8.0 

## Setup 
Tento návod je určený na sprevádzkovanie na OS Windows, kde aplikácia bude spustená ako Windows Service (iné spôsoby hostovania tu: https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/?view=aspnetcore-8.0) za predpokladu že proxy server je nastavený (adresa a port sa dajú nastaviť v appsettings.json v "Kestrel" pod "Url") 

### Databáza 
- vytvoriť databázu (prednastavený názov databázy je "fvl-radenie-vlakov") 
- vykonať transakciu na vytvorenie tabuliek (https://github.com/TIS2023-FMFI/vlaky/blob/main/docs/databaza/DB_CREATE.txt) 

### Service setup 
- Vytvoriť užívateľa pod ktorým aplikácia bude spustená a nastaviť mu pravá (https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/windows-service?view=aspnetcore-8.0&tabs=visual-studio#service-user-account) 
- Vytvorenie samotného Windows service pre aplikáciu (https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/windows-service?view=aspnetcore-8.0&tabs=visual-studio#create-and-manage-the-windows-service) 

## Nastavenia 
Všetky nastavenia sa nachádzajú v súbore appsettings.json 

### Nastavenie endpoint-ov 
V sekcii “Kestrel” sa dá nastaviť na akej adrese a porte aplikácia počúva zmenením hodnoty v “Url”, ak sa jedna o localhost adresu tak automaticky nastaví aj 127.0.0.1 s rovnakým portom (toto môže zlyhať v prípade že daný port je požívaný iným service) 

### Informácie na pripojenie na Databázu 
Nastaviť ako a kam sa aplikácia pripojuje na databázu sa da nastaviť upravením hodnoty “DefaultConnection” v sekcii “ConnectionStrings”, ako ich nastaviť a ďalšie iné nastavenia nájdete na https://www.npgsql.org/doc/connection-string-parameters.html 

### Nastavenie výstupu logu 
Presnú formuláciu výpisu do logu sa da upraviť v sekcii “FileLoggingOptions”. Navrhnutá formulácia je rozpísaná v docs/Navrh.pdf (https://github.com/TIS2023-FMFI/vlaky/blob/main/docs/N%C3%A1vrh.pdf) v kapitole 3 v sekcii Logova aktivita 

 