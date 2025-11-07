import { Server } from "@colyseus/core";
import { ThrowingSimulatorRoom } from "./rooms/ThrowingSimulatorRoom";

export default {
    initializeGameServer: (gameServer: Server) => {
        gameServer.define("my_room", ThrowingSimulatorRoom);
    }
};