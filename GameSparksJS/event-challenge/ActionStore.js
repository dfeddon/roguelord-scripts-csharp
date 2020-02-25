// ====================================================================================================
//
// Cloud Code for ActionStore, write your code here to customize the GameSparks platform.
//
// For details of the GameSparks Cloud Code API see https://docs.gamesparks.com/
//
// ====================================================================================================
requireOnce("CombatResultsModule");

//////////////////////////////////////////////////////////////////
// Constants
//////////////////////////////////////////////////////////////////
const PRIVATE_CHALLENGED_ACTIONS = "challengedActions";
const PRIVATE_CHALLENGER_ACTIONS = "challengerActions";

const PUBLIC_CHALLENGED_TACTICS = "challengedTactics";
const PUBLIC_CHALLENGED_PHASE = "challengedPhase";

const PUBLIC_CHALLENGER_TACTICS = "challengerTactics";
const PUBLIC_CHALLENGER_PHASE = "challengerPhase";

const PUBLIC_WEATHER = "weather";

const STORE_ACTIONSDATA = "actionData";

//////////////////////////////////////////////////////////////////
// Init
//////////////////////////////////////////////////////////////////

var data = Spark.getData();
var challengeId = data.challengeInstanceId;

// get player id (sender)
var playerId = Spark.getPlayer().getPlayerId();

// get challenge
var challenge = Spark.getChallenge(challengeId);
// get challenger
var challengerId = challenge.getChallengerId();
// var players = challenge.getChallengedPlayerIds();

//////////////////////////////////////////////////////////////////
// if challenged, just store data to privateData
//////////////////////////////////////////////////////////////////
if (playerId !== challengerId) {
    // clear extant combat results from script data
    challenge.removeScriptData("combatResults");
    
    // clear private data
    // challenge.removeScriptData("challengeeActions");
    // challenge.removeScriptData("challengeeTactics");
    // challenge.removeScriptData("challengeePhase");
    // load challenger player
    // var challenger = Spark.loadPlayer(playerid);
    // first, let's setup scriptData objects (if not already)
    var publicChallengedCrewMods = challenge.getScriptData(PUBLIC_CHALLENGED_TACTICS);
    if (publicChallengedCrewMods === null) {
        // define fixed mods (tactics)
        var tactics = {};
        challenge.setScriptData(PUBLIC_CHALLENGED_TACTICS, {});
        challenge.setScriptData(PUBLIC_CHALLENGER_TACTICS, {});
    }

    // next, update dynamic crew mods: phase, weather, crew cards, etc.
    challenge.setScriptData(PUBLIC_CHALLENGED_PHASE, {});
    challenge.setScriptData(PUBLIC_CHALLENGER_PHASE, {});
    challenge.setScriptData(PUBLIC_WEATHER, {});

    // lastly, let's save the action data
    challenge.setPrivateData(PRIVATE_CHALLENGED_ACTIONS, data);
    // var privatedata = challenge.getPrivateData("actionData");
} 
//////////////////////////////////////////////////////////////////
// challenger data, process combat results...
//////////////////////////////////////////////////////////////////
else {
    
    // load challenged player
    // var challenger = Spark.loadPlayer(playerid);

    // first, confirm we have both challenger & challenged data
    var challenged = challenge.getPrivateData(PRIVATE_CHALLENGED_ACTIONS);
    // confirm phase number is synched
    if (challenged.ActionData.phase !== data.ActionData.phase) {
        // Spark.exit();
    }
    // next, combine privateData (challenged) with data.ActionData (challenger)
    challenge.setPrivateData(PRIVATE_CHALLENGER_ACTIONS, data.ActionsData);

    //////////////////////////////////////////////////////////////////
    // store actions in Data Explorer
    //////////////////////////////////////////////////////////////////
    var privatedata = challenge.getPrivateData(PRIVATE_CHALLENGED_ACTIONS);
    var API = Spark.getGameDataService();
    var entryName = challengeId + data.ActionData.phase;
    var entry = API.createItem(STORE_ACTIONSDATA, entryName);

    // data to store
    var datastore = entry.getData();
    datastore.challenged = privatedata.ActionData;
    datastore.challengedPhaseType = 1;
    datastore.challenger = data.ActionData;
    datastore.challengerPhaseType = 2;
    datastore.phase = data.ActionData.phase;
    datastore.weather = 0;
    datastore.timestamp = new Date().toISOString();

    // store it
    var status = entry.persistor().persist().error();

    //////////////////////////////////////////////////////////////////
    // error handling
    //////////////////////////////////////////////////////////////////
    if (status) {
        Spark.setScriptError("ERROR", status);
        //Stop execution of script
        Spark.exit();
    }

    // var entry2 = API.getItem("actionData", entryName);
    // var doc = entry2.document();
    // var data2 = doc.getData();

    //////////////////////////////////////////////////////////////////
    // process combat results: player id, player data, opponent id, opponent data (stored), phase
    //////////////////////////////////////////////////////////////////
    CombatResultsModule(playerId, data.ActionData, data.ActionData.opponent, privatedata.ActionData, data.ActionData.phase);
}
