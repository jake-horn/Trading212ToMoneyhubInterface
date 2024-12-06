# Trading212ToMoneyhubInterface

Moneyhub doesn't have an API for me to update manual accounts, so instead I've decided to simulate a browser to update these values for me in a fairly overengineered application that takes the Trading212 data via the API (which is incredibly useful, thanks Trading212!) and updates Moneyhub. 

If you use this, you'll need to disable Moneyhub's MFA, otherwise every login will require you to provide the MFA code and completely ruins the application. 

To run: 
- Clone or download the files
- Build the application
- Update the appsettings.json with the correct information. 
- Run. 

I've set up a scheduled task to run this each night, you could run this as often as you like but you might set alarm bells off at Moneyhub if you're updating it every minute. 