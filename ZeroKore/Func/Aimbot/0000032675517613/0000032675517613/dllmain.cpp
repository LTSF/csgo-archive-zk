#include <iostream>
#include "class.h"
#include "C:\Users\Alex\Documents\VS Projects\Hybrid\kMH\offsets.h"

const int SCREEN_WIDTH = GetSystemMetrics(SM_CXSCREEN); const int xhairx = SCREEN_WIDTH / 2;
const int SCREEN_HEIGHT = GetSystemMetrics(SM_CYSCREEN); const int xhairy = SCREEN_HEIGHT / 2;

int ClosestTarget;

KDRIV kDriver = KDRIV("\\\\.\\sovietsky");

DWORD PID = kDriver.GetPID();
DWORD CLIENT = kDriver.GetClient();

class Vector3 {
public:
	float x, y, z;
	Vector3() : x(0.f), y(0.f), z(0.f) {}
	Vector3(float _x, float _y, float _z) : x(_x), y(_y), z(_z) {}
};

DWORD GetTeam(DWORD Player)
{
	return kDriver.RVM<DWORD>(PID, Player + m_iTeamNum, sizeof(DWORD));
}

DWORD GetLocalPlayer()
{
	return kDriver.RVM<DWORD>(PID, CLIENT + dwLocalPlayer, sizeof(DWORD));
}

DWORD GetPlayerByIndex(int Index)
{
	return kDriver.RVM<DWORD>(PID, CLIENT + dwEntityList + Index * 0x10, sizeof(DWORD));
}

DWORD GetHealth(DWORD Player)
{
	return kDriver.RVM<DWORD>(PID, Player + m_iHealth, sizeof(DWORD));
}

BOOL GetDormantState(DWORD Player)
{
	return kDriver.RVM<BOOL>(PID, Player + m_bDormant, sizeof(BOOL));
}

Vector3 GetLocation(DWORD Player)
{
	return kDriver.RVM<Vector3>(PID, Player + m_vecOrigin, sizeof(Vector3));
}

Vector3 GetHeadPosition(DWORD Player)
{
	struct boneMatrix_t {
		byte pad3[12];
		float x;
		byte pad1[12];
		float y;
		byte pad2[12];
		float z;
	};
	DWORD BoneMatrixBase = kDriver.RVM<DWORD>(PID, Player + m_dwBoneMatrix, sizeof(DWORD));
	boneMatrix_t BoneMatrix = kDriver.RVM<boneMatrix_t>(PID, BoneMatrixBase + (sizeof(BoneMatrix) * 8), sizeof(boneMatrix_t));
	return Vector3(BoneMatrix.x, BoneMatrix.y, BoneMatrix.z);
}

struct view_matrix_t {
	float matrix[16];
} vm;

struct Vector3 WorldToScreen(const struct Vector3 pos, struct view_matrix_t matrix)
{
	struct Vector3 out;
	float _x = matrix.matrix[0] * pos.x + matrix.matrix[1] * pos.y + matrix.matrix[2] * pos.z + matrix.matrix[3];
	float _y = matrix.matrix[4] * pos.x + matrix.matrix[5] * pos.y + matrix.matrix[6] * pos.z + matrix.matrix[7];
	out.z = matrix.matrix[12] * pos.x + matrix.matrix[13] * pos.y + matrix.matrix[14] * pos.z + matrix.matrix[15];

	_x *= 1.f / out.z;
	_y *= 1.f / out.z;

	out.x = SCREEN_WIDTH * .5f;
	out.y = SCREEN_HEIGHT * .5f;

	out.x += 0.5f * _x * SCREEN_WIDTH + 0.5f;
	out.y -= 0.5f * _y * SCREEN_HEIGHT + 0.5f;

	return out;
}

float PT(int x1, int y1, int x2, int y2) {
	return sqrt(pow(x2 - x1, 2) + pow(y2 - y1, 2));
}

DWORD FindClosestEnemy()
{
	float Finish;
	int ClosestEnt = 0;

	Vector3 Calc = { 0, 0, 0 };
	float Closest = FLT_MAX;

	DWORD LocalTeam = GetTeam(GetLocalPlayer());

	for (int i = 1; i < 64; i++)
	{
		DWORD CurrentEntity = GetPlayerByIndex(i);
		DWORD CurrentTeam = GetTeam(CurrentEntity);
		DWORD CurrentHealth = GetHealth(CurrentEntity);
		BOOL CurrentDormancy = GetDormantState(CurrentEntity);
		Vector3 CurrentHeadBone = WorldToScreen(GetHeadPosition(CurrentEntity), vm);

		if (CurrentTeam == LocalTeam || CurrentDormancy || CurrentHealth > 100 || CurrentHealth < 1)
			continue;

		Finish = PT(CurrentHeadBone.x, CurrentHeadBone.y, xhairx, xhairy);

		if (Finish < Closest)
		{
			Closest = Finish;
			ClosestEnt = i;
		}
	}

	return ClosestEnt;
}

void FindClosestT()
{
	while (true)
		ClosestTarget = FindClosestEnemy();
}

float GetDistance(Vector3 from, Vector3 to)
{
	Vector3 delta = Vector3{ from.x - to.x, from.y - to.y, from.z - to.z };
	return sqrt(delta.x * delta.x + delta.y * delta.y + delta.z * delta.z);
}


extern "C" __declspec(dllexport) void Aimbot()
{
	CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)FindClosestT, NULL, NULL, NULL);

	while (true)
	{
		vm = kDriver.RVM<view_matrix_t>(PID, CLIENT + dwViewMatrix, sizeof(view_matrix_t));

		DWORD GlowObject = kDriver.RVM<DWORD>(PID, CLIENT + dwGlowObjectManager, sizeof(DWORD));
		DWORD Entity = kDriver.RVM<DWORD>(PID, CLIENT + dwEntityList + ClosestTarget * 0x10, sizeof(DWORD));
		DWORD eGlow = kDriver.RVM<DWORD>(PID, Entity + m_iGlowIndex, sizeof(DWORD));

		Vector3 MyLoc = GetLocation(GetLocalPlayer());
		Vector3 YourLoc = GetLocation(Entity);

		float Distance = GetDistance(YourLoc, MyLoc);

		Vector3 ClosestW2SHead = WorldToScreen(GetHeadPosition(GetPlayerByIndex(ClosestTarget)), vm);

		kDriver.WVM<FLOAT>(PID, GlowObject + eGlow * 0x38 + 0x4, 1, sizeof(FLOAT));
		kDriver.WVM<FLOAT>(PID, GlowObject + eGlow * 0x38 + 0x8, 1, sizeof(FLOAT));
		kDriver.WVM<FLOAT>(PID, GlowObject + eGlow * 0x38 + 0xC, 1, sizeof(FLOAT));
		kDriver.WVM<FLOAT>(PID, GlowObject + eGlow * 0x38 + 0x10, 0.7f, sizeof(FLOAT));

		kDriver.WVM<BOOL>(PID, GlowObject + eGlow * 0x38 + 0x24, true, sizeof(BOOL));
		kDriver.WVM<BOOL>(PID, GlowObject + eGlow * 0x38 + 0x25, false, sizeof(BOOL));

		if (GetAsyncKeyState(VK_MENU) && ClosestW2SHead.z >= 0.001f && ClosestTarget != 0)
		{
			kDriver.WVM<FLOAT>(PID, GlowObject + eGlow * 0x38 + 0x4, 0, sizeof(FLOAT));
			kDriver.WVM<FLOAT>(PID, GlowObject + eGlow * 0x38 + 0x8, 1, sizeof(FLOAT));
			kDriver.WVM<FLOAT>(PID, GlowObject + eGlow * 0x38 + 0xC, 1, sizeof(FLOAT));
			kDriver.WVM<FLOAT>(PID, GlowObject + eGlow * 0x38 + 0x10, 0.7f, sizeof(FLOAT));

			kDriver.WVM<BOOL>(PID, GlowObject + eGlow * 0x38 + 0x24, true, sizeof(BOOL));
			kDriver.WVM<BOOL>(PID, GlowObject + eGlow * 0x38 + 0x25, false, sizeof(BOOL));

			if (Distance > 800)
				SetCursorPos(ClosestW2SHead.x, ClosestW2SHead.y + 13);
			else if (Distance > 200 && Distance < 500)
				SetCursorPos(ClosestW2SHead.x, ClosestW2SHead.y + 7);
			else
				SetCursorPos(ClosestW2SHead.x, ClosestW2SHead.y);

			BlockInput(true);
			Sleep(35);
		}

		BlockInput(false);
	}
}