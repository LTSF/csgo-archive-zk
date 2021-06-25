#include "class.h"
#include "C:\Users\Alex\Documents\VS Projects\Hybrid\ZeroKore\offsets.h"

extern "C" __declspec(dllexport) void AutoShoot()
{
	KDRIV kDriver = KDRIV("\\\\.\\sovietsky");

	DWORD PID = kDriver.GetPID();
	DWORD CLIENT = kDriver.GetClient();

	while (true)
	{
		DWORD LocalPlayer = kDriver.RVM<DWORD>(PID, CLIENT + dwLocalPlayer, sizeof(DWORD));
		DWORD lpTeam = kDriver.RVM<DWORD>(PID, LocalPlayer + m_iTeamNum, sizeof(DWORD));
		DWORD XHID = kDriver.RVM<DWORD>(PID, LocalPlayer + m_iCrosshairId, sizeof(DWORD));

		if (XHID != 0 && XHID <= 64)
		{
			DWORD VictimPlayer = kDriver.RVM<DWORD>(PID, CLIENT + dwEntityList + ((XHID - 1) * 0x10), sizeof(DWORD));

			if (VictimPlayer)
			{
				DWORD vpTeam = kDriver.RVM<DWORD>(PID, VictimPlayer + m_iTeamNum, sizeof(DWORD));
				DWORD vpHealth = kDriver.RVM<DWORD>(PID, VictimPlayer + m_iHealth, sizeof(DWORD));
				
				if (lpTeam != vpTeam && vpHealth > 0)
				{
					Sleep(40);
					kDriver.WVM<__int64>(PID, CLIENT + dwForceAttack, 0x5, 8);
					Sleep(10);
					kDriver.WVM<__int64>(PID, CLIENT + dwForceAttack, 0x4, 8);
				}
			}
		}
		Sleep(50);
	}
}