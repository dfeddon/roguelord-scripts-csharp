// ====================================================================================================
//
// Cloud Code for CombatResultsModule, write your code here to customize the GameSparks platform.
//
// For details of the GameSparks Cloud Code API see https://docs.gamesparks.com/
//
// ====================================================================================================
/*
    parameters:

    ActionData.data: 
    [
        { 
            id: character id
            o:  order
            s:  source id
            t:  target id
            a:  ability id
            c:  array of cards { id, [l]evel }
        }
    ]
    
    Player.privateData.crew: (challenged & challenger)
    [
        {
            id: characterId
            ar: archetype
            at: attributes { [r]eflexes, [g]rit, [f]ocus, [m]eat, [p]recision }
            h:  health
            a:  attack
            d:  defense
            sm: state of mind
            a1: weapon id
            a2: weapon mod id
            a3: gear mod id
            a4: gear id
            b:  banked power
            sc: sidecar { id, phases remaining }
            e:  array of effects { id, [s]tacks }
            c:  array of cards { id, [l]evel }
            sk: array of sockets { id, [l]evel }
        }
    ]

    Player.privateData.tactics:
    [
        {
            id: tactic id
            l:  level
        }
    ]
    
    Challenge.scriptData.weather: 
    {
        
    }
    Challenge.scriptData.heat:
    {
        t: heat template
        p: array of phases { type }
        c: current phase index
    }
    
    culled from Spark.player privateData
    challengerTactics: {}
    challengerPhase: {}
    challengedTactics: {}
    challengedPhase: {}
    
    culled from Spark.challenge scriptData
    weather: {}
    challenged id
    challenged: array of actions
    phase: int
    
    returns:
    
    array of combatResultsVO
*/
function CombatResultsModule(challengerid, challengerData, challengedid, challengedData, phase) {
    //////////////////////////////////////////////////////////////////
    // prep data
    //////////////////////////////////////////////////////////////////
    var data = Spark.getData();
    var challengeId = data.challengeInstanceId;
    var challenge = Spark.getChallenge(challengeId);
    var playerChallenged = Spark.loadPlayer(challengedid);
    var playerChallenger = Spark.loadPlayer(challengerid);

    // test
    var API = Spark.getGameDataService();
    var entry = API.getItem("actionData", challengeId + "1");
    if (entry.error()) {
        Spark.setScriptError("ERROR", error);
        Spark.exit();
    } else {
        var data = entry.document().getData();
    }
    // metadata
    var metaWeapons = Spark.metaCollection("weapons");

    // concat challengerData and challengedData arrays
    var all = challengerData.data.concat(challengedData.data);

    //////////////////////////////////////////////////////////////////
    // sort by order
    //////////////////////////////////////////////////////////////////
    all.sort(sortOrder);
    var slain = []; // death array

    //////////////////////////////////////////////////////////////////
    // get phase & weather mods for each crew
    //////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////
    // get tactics mods for each crew
    //////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////
    // get crew offensive mods
    //////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////
    // get crew defensive mods
    //////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////
    // iterate
    //////////////////////////////////////////////////////////////////
    var results = [];
    var c, s, t, a, c1, c2, c3, result;
    for (var i = 0; i < all.length; i++) {
        result = {};
        // reset target
        t = 0;
        // get char
        c = all[i];
        // get char source
        s = c.s;
        // get char target
        if (c.t > 0)
            t = c.t;
        // get ability & cards
        a = c.a;
        c1 = c.c1;
        c2 = c.c2;
        c3 = c.c3;

        //////////////////////////////////////////////////
        // confirm whether source/target is slain 
        //////////////////////////////////////////////////
        for (var s = 0; s < slain.length; s++) {
            if (slain[s] === s) {
                result.s = { "isDead": true };
                results.push(result);
                continue;
            }
            else if (slain[s] === t) {
                result.t = { "isDead": true };
                results.push(result);
                continue;
            }
        }

        // testing
        results.push(c);

        // cull source data from db
        // cull target data from db
        // cull ability data from db
        // cull cards data from db

        // evaluate attacker mods (oop, weapon/gear/mods, SoM)
        // evaluate defender mods (oop, weapon/gear/mods, SoM)

        // actions validation
        // get extant effects, state of mind
        // process attacks, defenses, cards used (in turn order)
    }
    //////////////////////////////////////////////////
    // append results to scriptData
    //////////////////////////////////////////////////
    challenge.setScriptData("combatResults", results);
}

function sortOrder(a, b) {
    if (a.o < b.o) return -1;
    else if (a.o > b.o) return 1;
    else return 0;
}