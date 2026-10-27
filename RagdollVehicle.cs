using System;
using GTA;
using GTA.Native;

public class RagdollVehicle : Script
{
    public RagdollVehicle()
    {
        Tick += OnTick;
    }

    private void OnTick(object sender, EventArgs e)
    {
        Ped player = Game.Player.Character;
        
        // Buscamos los peatones cercanos alrededor del jugador
        foreach (Ped ped in World.GetNearbyPeds(player, 100f))
        {
            // Comprobamos si el NPC está en un vehículo, si está muerto o muriendo
            if (ped.Exists() && ped != player && ped.IsInAnyVehicle())
            {
                if (ped.IsDead || ped.Health <= 0)
                {
                    // Evitamos que el juego repita el script infinitamente en el mismo NPC
                    if (!ped.IsRagdoll)
                    {
                        // 1. Desvinculamos al NPC del anclaje del asiento de Rockstar
                        Function.Call(Hash.DETACH_ENTITY, ped.Handle, true, true);
                        
                        // 2. Activamos el sistema Ragdoll (físicas de ERO) dentro de la cabina
                        // Parámetros: tiempo (-1 infinito), tipo, dirección, etc.
                        ped.ToRagdoll(-1, -1, 0, false, false, false);
                    }
                }
            }
        }
    }
}
