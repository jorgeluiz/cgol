"use client"
import { useGameActions } from "@/hooks/game-hooks";

export default function GameLogicInitializer() {

    useGameActions();

    return null;
}