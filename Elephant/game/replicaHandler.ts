import {NextReplicaArgs, NextReplicaModel, ReplicaArgs, ReplicaModel} from "../engine/types";

const answers = [
    {id: "1", text: "Сам купи"},
    {id: "2", text: "Нет ты"},
]

export function getCurrentReplica(args: ReplicaArgs): ReplicaModel {
    return {
        text: args.state.lastMessage == null ? "Купи слона" : `Все говорят: ${args.state.lastMessage}, а ты купи слона`,
        takesFreeText: true,
        answers: [
            {id: "1", text: "Сам купи"},
            {id: "2", text: "Нет ты"},
        ],
    };
}

export function nextReplica(args: NextReplicaArgs): NextReplicaModel {
    if (typeof args.answerId != "string" && typeof args.answerText != "string") throw TypeError("at least one of answerId or answerText must be a string");
    const mes = typeof args.answerId == "string" ? answers.find(value => value.id == args.answerId).text : args.answerText;
    if (mes == undefined) throw TypeError("index of answer was incorrect")

    args.state.lastMessage = mes;

    const replica = getCurrentReplica({state: args.state, language: args.language, engineModule: args.engineModule})
    return {state: args.state, replica: replica}
}
