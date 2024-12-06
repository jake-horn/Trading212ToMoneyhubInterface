# Trading212ToMoneyhubInterface

Moneyhub doesn't have an API for me to update manual accounts, so instead I've decided to simulate a browser to update these values for me in a fairly overengineered application that takes the Trading212 data via the API (which is incredibly useful, thanks Trading212!) and updates Moneyhub. 

If you use this, you'll need to disable Moneyhub's MFA, otherwise every login will require you to provide the MFA code and completely ruins the application. 

To run: 
- Clone or download the files
- Update the appsettings.json (rename it from appsettings-example.json to appsettings.json) with the correct information.
- Build the application 
- Run. 

I've set up a scheduled task to run this each night, you could run this as often as you like but you might set alarm bells off at Moneyhub if you're updating it every minute. 

You'll need the Trading212 API key, you can find out how to get yours here: https://helpcentre.trading212.com/hc/en-us/articles/14584770928157-How-can-I-generate-an-API-key

You'll also need the account GUID from Moneyhub that needs updating. You can do this by: 
- Logging into Moneyhub on your browser
- Clicking "Accounts and Assets"
- Clicking through on your Trading212 manual account
- Get the GUID from the address: https://client.moneyhub.co.uk/#accounts/details/THE GUID IS HERE, COPY THIS SECTION
- Add this to the appsettings.json. 

You'll also need to add your Moneyhub username and password, and the location for the log files to go to. 