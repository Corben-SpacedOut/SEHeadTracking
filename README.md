# SEHeadTracking
Head Tracking Plugin for Space Engineers.

Head tracking is commonly used in flight simulator games to use head movements to look around. If you don't have a head tracking system, no worries! As long as you have a webcam, you cat easily get in on the action with the Aruco tracker.

## Demo Video

[![Demo Video on Youtube](https://github.com/Corben-SpacedOut/SEHeadTracking/raw/media/media/demo-thumb.png)  
Demo Video on Youtube](https://youtu.be/CP8tt_Na06c)

## Getting started

### Installation

The Head Tracking plugin is installed using the [Plugin Loader](https://steamcommunity.com/sharedfiles/filedetails/?id=2407984968). 
Install the Plugin Loader as per instructions and enable *Head Tracking* plugin from **Plugins** in the game main menu.

## Running with OpenTrack

The plugin has been tested with [opentrack](https://github.com/opentrack/opentrack/wiki). Select *freetrack 2.0* as the output.

Either start opentrack before you launch Space Engineers or during the game. Head tracking should start working immediately.

## Aruco tracker

If you do not yet have a head tracking setup, you can easily make one! Just print out the Aruco marker, glue it to you forehead and choose *aruco - paper marker tracker* as input in opentrack. It works better than you would expect!

![Aruco on forehead.](https://github.com/Corben-SpacedOut/SEHeadTracking/raw/media/media/per-aruco.png)

* [Opentrack instructions on Aruco](https://github.com/opentrack/opentrack/wiki/Aruco-tracker)
* [An excellent Youtube guide & review of the Aruco tracker by Sim Racing Corner.](https://www.youtube.com/watch?v=ajoUzwe1bT0)

### Test mode

If you don't have a head tracker or you just want to see the plugin working, you can enable head movement with the test mode.
To enable the test mode, press Enter to write a chat message and type
````
/ht_testmode on
````
