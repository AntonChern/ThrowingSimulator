import { Room, Client } from "@colyseus/core";
import { Vector_3, Vector_4, Player, Crate, ThrowingSimulatorState } from "./schema/ThrowingSimulatorState";

function getRandom(min: number, max: number): number {
    return Math.random() * (max - min) + min; // Inclusive of both min and max
}

export class ThrowingSimulatorRoom extends Room<ThrowingSimulatorState> {
    maxClients = 4;
    state = new ThrowingSimulatorState();

    onCreate(options: any) {
        const cratesNum = Math.floor(getRandom(5, 8.5));
        for (let i: number = 0; i < cratesNum; i++) {
            console.log("Iteration:", i);
            const crate = new Crate();
            crate.owner = "";
            crate.position = new Vector_3(getRandom(-7, 7), getRandom(1, 3), getRandom(-6, 1));
            crate.rotation = new Vector_4(0, 0, 0, 1);
            crate.scale = getRandom(0.6, 1);
            this.state.crates.push(crate);
        }

        this.onMessage("move_player", (client, message) => {
            this.state.lastChangedBy = client.sessionId;
            const player = this.state.players.get(message.id);
            if (player) {
                player.position = new Vector_3(message.posX, message.posY, message.posZ);
                player.rotation = new Vector_4(message.rotX, message.rotY, message.rotZ, message.rotW);
            }
        });

        this.onMessage("move_crate", (client, message) => {
            this.state.lastChangedBy = client.sessionId;
            const crate = this.state.crates[message.index];
            crate.position = new Vector_3(message.posX, message.posY, message.posZ);
            crate.rotation = new Vector_4(message.rotX, message.rotY, message.rotZ, message.rotW);
        });

        this.onMessage("interact_crate", (client, message) => {
            this.state.lastChangedBy = client.sessionId;
            const player = this.state.players.get(message.owner);
            const crate = this.state.crates[message.index];
            crate.author = message.owner;
            crate.owner = message.owner;
            if (message.isTaken) {
                player.crateIndex = message.index;
            }
            else {
                player.crateIndex = -1;
            }
        });

        this.onMessage("change_authority", (client, message) => {
            this.state.lastChangedBy = client.sessionId;
            const crate = this.state.crates[message.index];
            crate.author = message.author;
        });
    }

    onJoin(client: Client, options: any) {
        console.log(client.sessionId, "joined!");
        const player = new Player();
        player.crateIndex = -1;
        player.position = new Vector_3(getRandom(-7, 7), getRandom(1, 3), getRandom(-6, 1));
        player.rotation = new Vector_4(0, 1, 0, 0);
        this.state.players.set(client.sessionId, player);

        let i: number = 0;
        while (i < this.state.crates.length) {
            this.state.players.forEach((player, key) =>
            {
                if (i < this.state.crates.length) {
                    this.state.crates[i++].author = key;
                }
            });
        }

        for (let i: number = 0; i < this.state.crates.length; i++) {
            console.log(i, this.state.crates[i].author);
        }
    }

    onLeave(client: Client, consented: boolean) {
        console.log(client.sessionId, "left!");
        this.state.players.delete(client.sessionId);

        let i: number = 0;
        if (this.state.players.size != 0) {
            while (i < this.state.crates.length) {
                this.state.players.forEach((player, key) => {
                    if (i < this.state.crates.length) {
                        this.state.crates[i++].author = key;
                    }
                });
            }
        }
    }

    onDispose() {
        console.log("Room disposed!");
    }
}