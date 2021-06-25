#include "class.h"
#include "C:\Users\Alex\Documents\VS Projects\Hybrid\kMH\offsets.h"

extern "C" __declspec(dllexport) void GlowESP()
{
	KDRIV kDriver = KDRIV("\\\\.\\sovietsky");

	DWORD PID = kDriver.GetPID();
	DWORD CLIENT = kDriver.GetClient();

	while (true)
	{
		DWORD LocalPlayer = kDriver.RVM<DWORD>(PID, CLIENT + dwLocalPlayer, sizeof(DWORD));
		DWORD lpTeam = kDriver.RVM<DWORD>(PID, LocalPlayer + m_iTeamNum, sizeof(DWORD));

		DWORD GlowObject = kDriver.RVM<DWORD>(PID, CLIENT + dwGlowObjectManager, sizeof(DWORD));

		if (lpTeam == 0x1 || lpTeam == 0x0)
			continue;

		for (int i = 1; i < 64; i++)
		{
			DWORD Entity = kDriver.RVM<DWORD>(PID, CLIENT + dwEntityList + i * 0x10, sizeof(DWORD));

			if (Entity)
			{
				DWORD eTeam = kDriver.RVM<DWORD>(PID, Entity + m_iTeamNum, sizeof(DWORD));
				DWORD eGlow = kDriver.RVM<DWORD>(PID, Entity + m_iGlowIndex, sizeof(DWORD));
				BOOL eDormant = kDriver.RVM<BOOL>(PID, Entity + m_bDormant, sizeof(BOOL));

				if (eDormant)
					continue;

				if (lpTeam == eTeam)
				{
					kDriver.WVM<FLOAT>(PID, GlowObject + eGlow * 0x38 + 0x4, 0, sizeof(FLOAT));
					kDriver.WVM<FLOAT>(PID, GlowObject + eGlow * 0x38 + 0x8, 1, sizeof(FLOAT));
					kDriver.WVM<FLOAT>(PID, GlowObject + eGlow * 0x38 + 0xC, 0, sizeof(FLOAT));
					kDriver.WVM<FLOAT>(PID, GlowObject + eGlow * 0x38 + 0x10, 0.7f, sizeof(FLOAT));

					kDriver.WVM<BOOL>(PID, GlowObject + eGlow * 0x38 + 0x24, true, sizeof(BOOL));
					kDriver.WVM<BOOL>(PID, GlowObject + eGlow * 0x38 + 0x25, false, sizeof(BOOL));
				}
				else
				{
					kDriver.WVM<FLOAT>(PID, GlowObject + eGlow * 0x38 + 0x4, 1, sizeof(FLOAT));
					kDriver.WVM<FLOAT>(PID, GlowObject + eGlow * 0x38 + 0x8, 0, sizeof(FLOAT));
					kDriver.WVM<FLOAT>(PID, GlowObject + eGlow * 0x38 + 0xC, 0, sizeof(FLOAT));
					kDriver.WVM<FLOAT>(PID, GlowObject + eGlow * 0x38 + 0x10, 0.7f, sizeof(FLOAT));

					kDriver.WVM<BOOL>(PID, GlowObject + eGlow * 0x38 + 0x24, true, sizeof(BOOL));
					kDriver.WVM<BOOL>(PID, GlowObject + eGlow * 0x38 + 0x25, false, sizeof(BOOL));
				}
			}
		}
	}
}