import { Schema, MapSchema, ArraySchema, type } from "@colyseus/schema";

export class Vector_3 extends Schema {
    @type("number") x: number;
    @type("number") y: number;
    @type("number") z: number;

    constructor(x: number, y: number, z: number) {
        super();
        this.x = x;
        this.y = y;
        this.z = z;
    }
}

export class Player extends Schema {
    @type("number") crateIndex: number;
    @type(Vector_3) position: Vector_3;
    @type(Vector_3) rotation: Vector_3;
}

export class Crate extends Schema {
    @type("string") owner: string;
    @type("boolean") interactable: boolean;
    @type(Vector_3) position: Vector_3;
    @type(Vector_3) rotation: Vector_3;
    @type("number") scale: number;
}

export class ThrowingSimulatorState extends Schema {
    @type({ map: Player }) players = new MapSchema<Player>();
    @type([Crate]) crates = new ArraySchema<Crate>();
}