import {InitializerArgs, StateModel} from "../engine/types.js";

export function createInitState(args: InitializerArgs): StateModel {
    return {
        lastMessage: null
    }
}
