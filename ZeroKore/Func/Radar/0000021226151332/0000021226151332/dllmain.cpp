#include "class.h"
#include "C:\Users\Alex\Documents\VS Projects\Hybrid\ZeroKore\offsets.h"

extern "C" __declspec(dllexport) void Radar()
{
	KDRIV kDriver = KDRIV("\\\\.\\sovietsky");

	DWORD PID = kDriver.GetPID();
	DWORD CLIENT = kDriver.GetClient();

	while (true)
	{
		DWORD LocalPlayer = kDriver.RVM<DWORD>(PID, CLIENT + dwLocalPlayer, sizeof(DWORD));
		DWORD lpTeam = kDriver.RVM<DWORD>(PID, LocalPlayer + m_iTeamNum, sizeof(DWORD));

		for (int i = 1; i < 64; i++)
		{
			DWORD VictimPlayer = kDriver.RVM<DWORD>(PID, CLIENT + dwEntityList + (i * 0x10), sizeof(DWORD));
			DWORD vpTeam = kDriver.RVM<DWORD>(PID, VictimPlayer + m_iTeamNum, sizeof(DWORD));
			BOOL vpDormant = kDriver.RVM<BOOL>(PID, VictimPlayer + m_bDormant, sizeof(BOOL));
			BOOL vpSpotted = kDriver.RVM<DWORD>(PID, VictimPlayer + m_bSpotted, sizeof(BOOL));

			if (!vpDormant && vpTeam != lpTeam)
				if (!vpSpotted)
					kDriver.WVM<BOOL>(PID, VictimPlayer + m_bSpotted, true, sizeof(BOOL));
		}
		Sleep(50);
	}
}
