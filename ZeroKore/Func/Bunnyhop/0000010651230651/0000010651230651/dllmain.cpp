#include "class.h"
#include "C:\Users\Alex\Documents\VS Projects\Hybrid\ZeroKore\offsets.h"

extern "C" __declspec(dllexport) void Bunnyhop()
{
	KDRIV kDriver = KDRIV("\\\\.\\sovietsky");

	DWORD PID = kDriver.GetPID();
	DWORD CLIENT = kDriver.GetClient();

	while (true)
	{
		DWORD LocalPlayer = kDriver.RVM<DWORD>(PID, CLIENT + dwLocalPlayer, sizeof(DWORD));
		DWORD IsOnGround = kDriver.RVM<DWORD>(PID, LocalPlayer + m_fFlags, sizeof(DWORD));

		if ((GetAsyncKeyState(VK_SPACE) & 0x8000) && (IsOnGround & 1 == 1))
		{
			kDriver.WVM<__int64>(PID, CLIENT + dwForceJump, 0x5, 8);
			Sleep(30);
			kDriver.WVM<__int64>(PID, CLIENT + dwForceJump, 0x4, 8);
		}
		Sleep(50);
	}
}