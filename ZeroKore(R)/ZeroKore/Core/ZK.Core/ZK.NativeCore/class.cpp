#include "class.h"

KDRIV::KDRIV(LPCSTR RegistryPath)
{
	hDriver = CreateFileA(RegistryPath, GENERIC_READ | GENERIC_WRITE,
		FILE_SHARE_READ | FILE_SHARE_WRITE, 0, OPEN_EXISTING, 0, 0);
}

KDRIV::~KDRIV()
{
	if (hDriver != NULL)
		CloseHandle(hDriver);

	hDriver = NULL;
}

DWORD KDRIV::GetPID()
{
	if (hDriver == INVALID_HANDLE_VALUE)
		return false;

	ULONG Id;
	DWORD Bytes;

	if (DeviceIoControl(hDriver, IO_GET_ID_REQUEST, &Id, sizeof(Id),
		&Id, sizeof(Id), &Bytes, NULL))
		return Id;
	else
		return false;
}

DWORD KDRIV::GetClient()
{
	if (hDriver == INVALID_HANDLE_VALUE)
		return false;

	ULONG Address;
	DWORD Bytes;

	if (DeviceIoControl(hDriver, IO_GET_CLIENT_REQUEST, &Address, sizeof(Address),
		&Address, sizeof(Address), &Bytes, NULL))
		return Address;
	else
		return false;
}

DWORD KDRIV::GetEngine()
{
	if (hDriver == INVALID_HANDLE_VALUE)
		return false;

	ULONG Address;
	DWORD Bytes;

	if (DeviceIoControl(hDriver, IO_GET_ENGINE_REQUEST, &Address, sizeof(Address),
		&Address, sizeof(Address), &Bytes, NULL))
		return Address;
	else
		return false;
}