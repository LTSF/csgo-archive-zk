#pragma once

#include <Windows.h>

#define IO_READ_REQUEST CTL_CODE(FILE_DEVICE_UNKNOWN, 0xF0A, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define IO_WRITE_REQUEST CTL_CODE(FILE_DEVICE_UNKNOWN, 0xF0B, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define IO_GET_ID_REQUEST CTL_CODE(FILE_DEVICE_UNKNOWN, 0xF0C, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define IO_GET_CLIENT_REQUEST CTL_CODE(FILE_DEVICE_UNKNOWN, 0xF0D, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)
#define IO_GET_ENGINE_REQUEST CTL_CODE(FILE_DEVICE_UNKNOWN, 0xF0E, METHOD_BUFFERED, FILE_SPECIAL_ACCESS)

typedef struct _KERNEL_READ_REQUEST
{
	ULONG ProcessId;

	ULONG Address;
	PVOID Response;
	ULONG Size;

} KERNEL_READ_REQUEST, * PKERNEL_READ_REQUEST;

typedef struct _KERNEL_WRITE_REQUEST
{
	ULONG ProcessId;

	ULONG Address;
	PVOID Value;
	ULONG Size;

} KERNEL_WRITE_REQUEST, * PKERNEL_WRITE_REQUEST;

class KDRIV
{
public:
	KDRIV(LPCSTR RegistryPath);
	~KDRIV();

	template <typename type>
	type RVM(ULONG ProcessId, ULONG ReadAddress, SIZE_T Size)
	{
		/*if (hDriver == INVALID_HANDLE_VALUE)
			return NULL;*/

		DWORD Return, Bytes;
		KERNEL_READ_REQUEST ReadRequest;

		type Buffer;

		ReadRequest.ProcessId = ProcessId;
		ReadRequest.Address = ReadAddress;
		ReadRequest.Response = &Buffer;
		ReadRequest.Size = Size;


		if (DeviceIoControl(hDriver, IO_READ_REQUEST, &ReadRequest, sizeof(ReadRequest), &ReadRequest, sizeof(ReadRequest), 0, 0))
			return Buffer;
		else
			return Buffer;
	}

	template <typename type>
	bool WVM(ULONG ProcessId, ULONG WriteAddress, type WriteValue, SIZE_T WriteSize)
	{
		if (hDriver == INVALID_HANDLE_VALUE)
			return false;

		DWORD Bytes;

		KERNEL_WRITE_REQUEST  WriteRequest;
		WriteRequest.ProcessId = ProcessId;
		WriteRequest.Address = WriteAddress;
		WriteRequest.Value = &WriteValue;
		WriteRequest.Size = WriteSize;

		if (DeviceIoControl(hDriver, IO_WRITE_REQUEST, &WriteRequest, sizeof(WriteRequest), 0, 0, &Bytes, NULL))
			return true;
		else
			return false;
	}

	DWORD GetPID();
	DWORD GetClient();
	DWORD GetEngine();
private:
	HANDLE hDriver = NULL;
};
