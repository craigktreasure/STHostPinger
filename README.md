# SmartThings Host Pinger

**Using a .Net Core client application running on any computer (Windows, Mac or Linux), ping any IP, host name, or URL and pass this to a SmartApp in SmartThings!**

**Code and Program Location:**
https://github.com/jebbett/STHostPinger

## Requirements

**- [.Net Core 1.1 Runtime](https://www.microsoft.com/net/download/core#/runtime)** - This allows you to run the app on Windows, Linux, or Mac.

**- SmartThingsHostPinger.zip** - .Net Core client application for Windows, Linux, and Mac. The config.json needs to be configured with details as described below and can be running on any computer connected to the internet.

**- Host Pinger SmartApp** - This passes the output of the client app to the custom device types.

**- Host Ping Device Type (optional)** - This device type revieves the online/offline event and translates this in to an on/off switch or presence sensor.

## Version Control

 * 28/10/16 - 1.0 - Release Version
 * 29/10/16 - 1.1 - Added child app and triggering of external switches
 * 30/10/16 - 1.2 - Removed direct triggering of child devices, Child app now includes delay on 'Offline'

## How To Install:

### 1. Install the Smart App and Custom Device Type

Go to your IDE, then...

#### My SmartApps:

A. Create New SmartApp

B. Select “From Code”

C. Paste App source code with the code from the file SmartApp_HostPinger.txt.

D. Save and publish the app for yourself.

E. Enable OAuth in IDE by going in to IDE > My Smart Apps > [App Name] > App Settings > Enable OAuth.

F. Get the Token and API Endpoint values via one of the below methods:

* EASY OPTION: Enable debugging, open live logging in IDE and then open the app again and press done and values will be returned in Live Logging.
* Open the SmartApp and click API Information (this will involve manually typing the codes)

#### My Device Handlers (optional):

A. Create New Device Handler

B. Select “From Code”

C. Paste App source code with the code from the file DeviceType_HostPinger.txt.

D. Save and publish the device for yourself.

### 2. Configure the Smart App

Configuration should be self explanatory however come back here with any questions.

Add a new device give it the same name you have specified as the host in config.config, you can open the device later and change to a different name if you like!

### 3. Configure SmartThingsHostPinger client app and config.json

A. Install [.Net Core 1.1 Runtime](https://www.microsoft.com/net/download/core#/runtime).

B. Download and extract the zip file.

C. Open the config.json file.

D. Fill in your access token, app id, and the Smart Things API endpoint from the previous section.

```JSON
"SmartAppConfig": {
    "Simulate": false,
    "AccessToken": "ReplaceMe",
    "AppId": "ReplaceMe",
    "SmartThingsApiEndpoint": "https://graph-na02-useast1.api.smartthings.com:443"
}
```

E. Be sure to also check that your IDE URL matches the SmartThingsApiEndpoint in config.json. If you have the URL from the app then this should be correct. If you were unable to get this from the app then you will need to copy from the IDE. It'll be something like "graph-na02-useast1.api.smartthings.com".

F. Configure all of the IPs / Hosts or Addresses you want to ping.

```JSON
"Hosts": [
    "google.com",
    "192.168.1.1",
    "SERVER1"
],
```

G. The polling interval, timeout and debug logging can also be configured to your liking or leave as the default values.

```JSON
"Debug": false,
"PingOptions": {
    "PingIntervalSeconds": 30,
    "TimeoutMilliseconds": 1000,
    "RetryAttempts": 5
},
```

### 4. Run the client app

You can now run the app and and this should push the updates to your ST Hub via the cloud.

Windows:
  1. Open Command Prompt or PowerShell.
  2. Run `.\Run.ps1` if using PowerShell, run `Run.cmd` if using Command Prompt, or run `dotnet .\SmartThingsHostPinger.dll`.

Linux and Mac:
  1. Navigate to the directory you extracted the zip file.
  2. Run `chmod -c u+x ./Run.sh` to give script execute permissions to yourself.
  3. Run `./Run.sh` or run `dotnet ./SmartThingsHostPinger.dll` to run the app.

If anything obvious isn't working you will recieve errors in the debug log (`log-{Date}.txt`), you can enable extra debugging by setting the `Debug` option to `true` in config.json.

### 5. Errors / Debugging

**Client App - SendGetRequest: the remote server returned an error: (403) Forbidden**
There is an issue with the URL in config.json re-check section 2D and 2E.

**No error in the client app and no "Event" present in the App**
This is because SmartThings has not received an event. If you ran the client app before setting up the devices in Smart Things, then the SmartApp probably missed the event. In this case, try restarting the client app to send up new events. This can also be because the App ID or Token are incorrect. If you re-installed the client app, these values may have been reset and need to be setup again.

**"Event" in the app, but hasn't triggered the switch to change.**
This is likely to that the Child Device has been configured with a name not matching the host in config.config.

**Live Logging - java.lang.NullPointerException: Cannot get property 'authorities' on null object @ line xx**
You have not enabled OAuth in the SmartApp