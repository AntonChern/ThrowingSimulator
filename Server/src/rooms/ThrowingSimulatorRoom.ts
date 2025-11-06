import { Room, Client } from "@colyseus/core";
import { Vector_3, Player, Crate, ThrowingSimulatorState } from "./schema/ThrowingSimulatorState";

function getRandom(min: number, max: number): number {
    return Math.random() * (max - min) + min; // Inclusive of both min and max
}

export class ThrowingSimulatorRoom extends Room<ThrowingSimulatorState> {
    maxClients = 4;
    state = new ThrowingSimulatorState();

    onCreate(options: any) {

        const itemsNum = Math.floor(getRandom(5, 8.5));
        for (let i: number = 0; i < itemsNum; i++) {
            console.log("Iteration:", i);
            const crate = new Crate();
            crate.interactable = true;
            crate.owner = "";
            crate.position = new Vector_3(getRandom(-7, 7), getRandom(1, 3), getRandom(-6, 1));
            crate.rotation = new Vector_3(getRandom(-180, 180), getRandom(180, 180), getRandom(-180, 180));
            crate.scale = getRandom(0.5, 1);
            this.state.crates.push(crate);
        }

        this.onMessage("move_player", (client, message) => {
            const player = this.state.players.get(message.id);
            if (player) {
                //player.crateIndex = message.crateIndex;
                player.position = new Vector_3(message.posX, message.posY, message.posZ);
                player.rotation = new Vector_3(message.rotX, message.rotY, message.rotZ);
            }
            this.state.players.forEach((player, key) =>
            {
                console.log(key, player.position.x, player.position.y, player.position.z);
            });
            console.log();
        });

        this.onMessage("move_crate", (client, message) => {
            const crate = this.state.crates[message.index];
            //crate.interactable = true;
            //crate.owner = "";
            crate.position = new Vector_3(message.posX, message.posY, message.posZ);
            crate.rotation = new Vector_3(message.rotX, message.rotY, message.rotZ);
            //console.log(crate.interactable, crate.owner);
        });

        this.onMessage("interact_crate", (client, message) => {
            const player = this.state.players.get(message.owner);
            const crate = this.state.crates[message.index];
            crate.interactable = message.interactable;
            crate.owner = message.owner;
            if (message.isTaken) {
                player.crateIndex = message.index;
            }
            else {
                player.crateIndex = -1;
            }
            //crate.position = new Vector_3(message.posX, message.posY, message.posZ);
            //crate.rotation = new Vector_3(message.rotX, message.rotY, message.rotZ);
            console.log(crate.interactable, crate.owner);
        });
    }

    onJoin(client: Client, options: any) {
        console.log(client.sessionId, "joined!");
        const player = new Player();
        player.crateIndex = -1;
        player.position = new Vector_3(getRandom(-7, 7), getRandom(1, 3), getRandom(-6, 1));
        player.rotation = new Vector_3(0, 180, 0);
        this.state.players.set(client.sessionId, player);
    }

    onLeave(client: Client, consented: boolean) {
        console.log(client.sessionId, "left!");
        this.state.players.delete(client.sessionId);
    }

    onDispose() {
        console.log("Room disposed!");
    }
}