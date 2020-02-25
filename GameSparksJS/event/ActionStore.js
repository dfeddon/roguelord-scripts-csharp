// ====================================================================================================
//
// Cloud Code for ActionStore, write your code here to customize the GameSparks platform.
//
// For details of the GameSparks Cloud Code API see https://docs.gamesparks.com/
//
// ====================================================================================================
/*
var myActionData = Spark.getData().ActionData;

var challengeId = "5d77ada856f6d90520cb617f";//Spark.getData().challenge.challengeId;
var challenge = Spark.getChallenge(challengeId).startChallenge();

//Get playerId and timestamp to create entry name
var playerId = Spark.getPlayer().getPlayerId();

var time = new Date().toISOString();
var entryName = playerId + time;

//Create entry and get its data object
var API = Spark.getGameDataService();
var entry = API.createItem("actionData", entryName);
var data = entry.getData();

//Add new data to entry
data.myActionData = myActionData;
// data.trackName = trackName;
// data.timeTaken = timeTaken;
data.playerId = playerId;

//Persist and return any errors
var status = entry.persistor().persist().error();

if(status){
    Spark.setScriptError("ERROR", status);
}
*/