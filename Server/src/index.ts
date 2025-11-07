import http from 'http';
import express from 'express';
import cors from 'cors';
import { Server } from 'colyseus';
import { monitor } from '@colyseus/monitor';
import { ThrowingSimulatorRoom } from './rooms/ThrowingSimulatorRoom';

const PORT = Number(process.env.PORT || 2567);
const HOST = "0.0.0.0"; // Listen on all network interfaces
const app = express();

app.use(cors());
app.use(express.json());

const gameServer = new Server({
    server: http.createServer(app),
});

gameServer.define('my_room', ThrowingSimulatorRoom);

// Listen on all interfaces
gameServer.listen(PORT, HOST);

console.log(`Listening on ws://${HOST}:${PORT}`);
console.log(`Access this server from your network via your machine's IP address on port ${PORT}`);