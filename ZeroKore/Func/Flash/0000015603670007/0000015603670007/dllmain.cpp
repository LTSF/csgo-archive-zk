#include "class.h"
#include "C:\Users\Alex\Documents\VS Projects\Hybrid\ZeroKore\offsets.h"

extern "C" __declspec(dllexport) void AntiFlash()
{
	KDRIV kDriver = KDRIV("\\\\.\\sovietsky");

	DWORD PID = kDriver.GetPID();
	DWORD CLIENT = kDriver.GetClient();

	DWORD Duration = 0;

	while (true)
	{
		DWORD LocalPlayer = kDriver.RVM<DWORD>(PID, CLIENT + dwLocalPlayer, sizeof(DWORD));
		Duration = kDriver.RVM<DWORD>(PID, LocalPlayer + m_flFlashDuration, sizeof(DWORD));

		if (Duration > 0)
			kDriver.WVM<DWORD>(PID, LocalPlayer + m_flFlashDuration, 0, sizeof(DWORD));

		Sleep(50);
	}
}