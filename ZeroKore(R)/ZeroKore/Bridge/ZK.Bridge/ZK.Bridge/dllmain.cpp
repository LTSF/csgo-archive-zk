#include "class.h"

extern "C" __declspec(dllexport) int GetProcessID()
{
	KDRIV kDriver = KDRIV("\\\\.\\sovietsky");
	return kDriver.GetPID();
}

extern "C" __declspec(dllexport) int GetClientBaseAddress()
{
	KDRIV kDriver = KDRIV("\\\\.\\sovietsky");
	return kDriver.GetClient();
}

extern "C" __declspec(dllexport) int GetEngineBaseAddress()
{
	KDRIV kDriver = KDRIV("\\\\.\\sovietsky");
	return kDriver.GetEngine();
}


extern "C" __declspec(dllexport) int ReadInt32(int ReadAddress)
{
	KDRIV kDriver = KDRIV("\\\\.\\sovietsky");
	DWORD pID = kDriver.GetPID();

	return kDriver.RVM<int>(pID, ReadAddress, sizeof(int));
}

extern "C" __declspec(dllexport) bool WriteInt32(int WriteAddress, int Value)
{
	KDRIV kDriver = KDRIV("\\\\.\\sovietsky");
	DWORD pID = kDriver.GetPID();

	return kDriver.WVM<int>(pID, WriteAddress, Value, sizeof(int));
}

extern "C" __declspec(dllexport) long ReadInt64(int ReadAddress)
{
	KDRIV kDriver = KDRIV("\\\\.\\sovietsky");
	DWORD pID = kDriver.GetPID();

	return kDriver.RVM<long>(pID, ReadAddress, sizeof(long));
}

extern "C" __declspec(dllexport) bool WriteInt64(int WriteAddress, long Value)
{
	KDRIV kDriver = KDRIV("\\\\.\\sovietsky");
	DWORD pID = kDriver.GetPID();

	return kDriver.WVM<long>(pID, WriteAddress, Value, sizeof(long));
}

extern "C" __declspec(dllexport) float ReadFloat(int ReadAddress)
{
	KDRIV kDriver = KDRIV("\\\\.\\sovietsky");
	DWORD pID = kDriver.GetPID();

	return kDriver.RVM<float>(pID, ReadAddress, sizeof(float));
}

extern "C" __declspec(dllexport) bool WriteFloat(int WriteAddress, float Value)
{
	KDRIV kDriver = KDRIV("\\\\.\\sovietsky");
	DWORD pID = kDriver.GetPID();

	return kDriver.WVM<float>(pID, WriteAddress, Value, sizeof(float));
}

extern "C" __declspec(dllexport) bool ReadBool(int ReadAddress)
{
	KDRIV kDriver = KDRIV("\\\\.\\sovietsky");
	DWORD pID = kDriver.GetPID();

	return kDriver.RVM<bool>(pID, ReadAddress, sizeof(bool));
}

extern "C" __declspec(dllexport) bool WriteBool(int WriteAddress, bool Value)
{
	KDRIV kDriver = KDRIV("\\\\.\\sovietsky");
	DWORD pID = kDriver.GetPID();

	return kDriver.WVM<bool>(pID, WriteAddress, Value, sizeof(bool));
}

class Vector3 {
public:
	float x, y, z;
	Vector3() : x(0.f), y(0.f), z(0.f) {}
	Vector3(float _x, float _y, float _z) : x(_x), y(_y), z(_z) {}
};

extern "C" __declspec(dllexport) Vector3 ReadVector(int ReadAddress)
{
	KDRIV kDriver = KDRIV("\\\\.\\sovietsky");
	DWORD pID = kDriver.GetPID();

	return kDriver.RVM<Vector3>(pID, ReadAddress, sizeof(Vector3));
}

extern "C" __declspec(dllexport) bool WriteVector(int WriteAddress, Vector3 Value)
{
	KDRIV kDriver = KDRIV("\\\\.\\sovietsky");
	DWORD pID = kDriver.GetPID();

	return kDriver.WVM<Vector3>(pID, WriteAddress, Value, sizeof(Vector3));
}

struct BoneMatrix {
	byte pad3[12];
	float x;
	byte pad1[12];
	float y;
	byte pad2[12];
	float z;
};

extern "C" __declspec(dllexport) BoneMatrix ReadBM(int ReadAddress)
{
	KDRIV kDriver = KDRIV("\\\\.\\sovietsky");
	DWORD pID = kDriver.GetPID();

	return kDriver.RVM<BoneMatrix>(pID, ReadAddress, sizeof(BoneMatrix));
}

extern "C" __declspec(dllexport) bool WriteBM(int WriteAddress, BoneMatrix Value)
{
	KDRIV kDriver = KDRIV("\\\\.\\sovietsky");
	DWORD pID = kDriver.GetPID();

	return kDriver.WVM<BoneMatrix>(pID, WriteAddress, Value, sizeof(BoneMatrix));
}

struct ViewMatrix {
	float matrix[16];
};

extern "C" __declspec(dllexport) ViewMatrix ReadVM(int ReadAddress)
{
	KDRIV kDriver = KDRIV("\\\\.\\sovietsky");
	DWORD pID = kDriver.GetPID();

	return kDriver.RVM<ViewMatrix>(pID, ReadAddress, sizeof(ViewMatrix));
}

extern "C" __declspec(dllexport) bool WriteVM(int WriteAddress, ViewMatrix Value)
{
	KDRIV kDriver = KDRIV("\\\\.\\sovietsky");
	DWORD pID = kDriver.GetPID();

	return kDriver.WVM<ViewMatrix>(pID, WriteAddress, Value, sizeof(ViewMatrix));
}
