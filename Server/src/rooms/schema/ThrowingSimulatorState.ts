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

export class Vector_4 extends Schema {
    @type("number") x: number;
    @type("number") y: number;
    @type("number") z: number;
    @type("number") w: number;

    constructor(x: number, y: number, z: number, w: number) {
        super();
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }
}

export class Player extends Schema {
    @type("number") crateIndex: number;
    @type(Vector_3) position: Vector_3;
    @type(Vector_4) rotation: Vector_4;
}

export class Crate extends Schema {
    @type("string") author: string;
    @type("string") owner: string;
    @type(Vector_3) position: Vector_3;
    @type(Vector_4) rotation: Vector_4;
    @type("number") scale: number;
}

export class ThrowingSimulatorState extends Schema {
    @type("string") lastChangedBy: string;
    @type({ map: Player }) players = new MapSchema<Player>();
    @type([Crate]) crates = new ArraySchema<Crate>();
}