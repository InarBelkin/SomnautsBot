export interface StateModel {
    lastMessage: string | null;
}

export interface ReplicaArgs {
    language: string;
    state: StateModel;
    engineModule: object | null;
}

export interface NextReplicaArgs {
    answerId: string | null;
    answerText: string | null;
    language: string;
    state: StateModel;
    engineModule: object | null;
}

export interface ReplicaModel {
    text: string;
    takesFreeText: boolean;
    answers: AnswerModel[];
}

export interface NextReplicaModel {
    replica: ReplicaModel;
    state: StateModel;
}

export interface AnswerModel {
    id: string;
    text: string;
}

export interface InitializerArgs {
    engineModule: object | null;
}
