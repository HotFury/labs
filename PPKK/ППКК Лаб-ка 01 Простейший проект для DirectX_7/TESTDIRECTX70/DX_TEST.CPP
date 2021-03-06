//     ������ ���������������� Windows-����������      // 
// �� ���� ������� DirectDraw �� ���������� DirectX    //
//              ���. �.�. ������                       //
//------------------------------------------------------
// ����� �� ��������� �����������: �� ������� ������� VK_SPACE(������)

#define STRICT              // ��� ����������� ������ � � MFC
#define WIN32_LEAN_AND_MEAN // ��� ��������� ������� ��������

#include <windows.h>
//#include <windowsx.h>     // ������� ��������� Win-���������

#include <ddraw.h>    //��� ������� � DirectX
#include <mmsystem.h> //��� ������� � Multimedia API's DirectX

#define ClassName "DX_Window"            // ��� ������ �������� ����
#define AppName "DirectDraw Application" // ����� ��������� ����

HWND hWndG;  //���������� (��� ������������� �������� hWnd � ������ ��������)

//!!!!! ���������� pointer's �� ���������� DirectDraw !!!!!!!!!
LPDIRECTDRAW7 pDD; 
LPDIRECTDRAWSURFACE7 pPrimarySurface;
LPDIRECTDRAWSURFACE7 pPicFrames;
LPDIRECTDRAWSURFACE7 pFPS;
LPDIRECTDRAWSURFACE7 pBackBuffer;
LPDIRECTDRAWPALETTE  pDDPal;

//��� ���������� �� ����� ������������ ����� 
char* pFileName ="Example.bmp"; 
int frame=0;

//�������������� ��������� ��� ���������
#define MAX_HEIGHT 480  // ��������  ���������� ������ 
#define MAX_WIDTH 640   // == 
#define COLOR_DEPTH 8   // 8���/pixel
#define TRASPARENT_COLOR 0xFF//��� "�������� ������"

#define FRAME_HEIGHT 240 // ������� ����������� ����������� 
#define FRAME_WIDTH 320  // ==

#define FPS_HEIGHT 40    // ������� ����������� ����������� 
#define FPS_WIDTH 60     // ==

// ��������� ������� ���������
BOOL InitDirectDraw (HWND);    //������������� DirectDraw
void ErrorHandle(HWND,LPCTSTR);//�������, ���������� � ������ ������
BOOL CreateSurfaces();         //�������� ������������ 
LPDIRECTDRAWPALETTE CreateDirectDrawPalette(LPDIRECTDRAW7);//== �������
BOOL PrepareSurfaces();        //���������� ������������ � ������      
BOOL LoadBMP(LPDIRECTDRAWSURFACE7,char*);//�������� �����. �� BMP-�����
void PrepareFrame();           //�������� ����� �� "������"
BOOL ClearSurface(LPDIRECTDRAWSURFACE7);//�������� �����������
void DrawFrame();              //����� ����������� �� �����
void RemoveDirectDraw();       //�������� ���� �����������, ����� � DirectDraw
LRESULT CALLBACK WndProc(HWND, UINT, WPARAM, LPARAM);//������� ������� �������

// ����� ����� � ���������
int WINAPI WinMain(HINSTANCE hInst, HINSTANCE hPrevInst, 
				   LPSTR lpszCmdLine, int nCmdShow)
{
	WNDCLASSEX wndClass;
	HWND hWnd;
	MSG msg;
//-------------------------------------------------------
	wndClass.cbSize       =sizeof(wndClass);
	wndClass.style        =CS_HREDRAW|CS_VREDRAW;
	wndClass.lpfnWndProc  =WndProc;
	wndClass.cbClsExtra   =0;
	wndClass.cbWndExtra   =0;
	wndClass.hInstance    =hInst;
	wndClass.hIcon        =LoadIcon(NULL,IDI_WINLOGO);
	wndClass.hCursor      =LoadCursor(NULL,IDC_ARROW);
	wndClass.hbrBackground=NULL;//�������� ��������!!!
	wndClass.lpszMenuName =NULL;
	wndClass.lpszClassName=ClassName;
	wndClass.hIconSm      =LoadIcon(NULL,IDI_WINLOGO);

	RegisterClassEx(&wndClass);
//------------------------------------------------------
  hWndG=hWnd=CreateWindowEx(
		0, //WS_EX_TOPMOST,//�������������� ����� ����
		ClassName,	//����� ����
		AppName,	//����� ���������
		WS_POPUP,	//����� ����
		0,0,		//���������� X � Y
		GetSystemMetrics(SM_CXSCREEN),
		GetSystemMetrics(SM_CYSCREEN),//������ � ������
		NULL,		//���������� ������������� ����
		NULL,		//���������� ����
		hInst,		//��������� ����������
		NULL);		//�������������� ������
//--------------------------------------------------------
	ShowWindow(hWnd,nCmdShow);
	UpdateWindow(hWnd);
//---------------------------------------------------------
   while(GetMessage(&msg,NULL,0,0)) 
   {TranslateMessage( &msg );
    DispatchMessage( &msg );  
   }
   return msg.wParam;
}  // ����� WinMain
//---------------------------------------------------------
//������� �������
LRESULT CALLBACK WndProc(HWND hWnd, UINT msg, 
			 WPARAM wParam, LPARAM lParam)
{
 HDC hDC;
 switch(msg)
 {
   case WM_CREATE:
//������������� ���������, ��������� � DirectDraw
//����� ����� ���������� CreateWindowEx()
	if (!InitDirectDraw(hWnd))
		return FALSE;
   break;

//��� ������� ������� ���������� ������ ���������
   case WM_KEYDOWN:
    if(wParam==VK_SPACE) DestroyWindow(hWnd);
   break;
	 
// ������ �� WM_PAINT: 
	case WM_PAINT:
     PAINTSTRUCT ps;  
     hDC = BeginPaint( hWnd, &ps );// !!! ����������� � ������ WM_PAINT
   //���������� ����������� �� ������
     DrawFrame();
	 EndPaint( hWnd, &ps );      // !!! ����������� � ����� WM_PAINT
    break;

  //������ � RAM ����� ����
 case WM_DESTROY:
	RemoveDirectDraw();
	PostQuitMessage(0);
    break;

 default: return DefWindowProc(hWnd,msg,wParam,lParam);
 }
return 0;
}
//---------------------------------------------------------
//������������� DirectDraw
//
BOOL InitDirectDraw (HWND hwnd)
{
	//���������� ��� ������������ �����
	HRESULT hRet;

	//�������� ��� ����������
	pPrimarySurface=NULL;
	pBackBuffer=NULL;
	pDDPal=NULL;

	pPicFrames=NULL;

	hWndG=hwnd; 

	//�������� ���������� IDirectDraw7
	hRet=DirectDrawCreateEx(NULL, (VOID**)&pDD, IID_IDirectDraw7, NULL);
	if (hRet!=DD_OK)
	{
		ErrorHandle(hWndG, "DirectDrawCreate");
		return (FALSE);
	}

	//��������� ������������� ������ ����������
	hRet=
	pDD->SetCooperativeLevel(hWndG, DDSCL_EXCLUSIVE | DDSCL_FULLSCREEN);
	if (hRet!=DD_OK)
	{
		ErrorHandle(hWndG,"SetCooperativeLevel");
		return (FALSE);
	}

	//��������� ������������ �����������
	hRet=
	pDD->SetDisplayMode(MAX_WIDTH, MAX_HEIGHT, COLOR_DEPTH, NULL, NULL);
	if (hRet!=DD_OK)
	{
		ErrorHandle(hWndG,"SetDisplayMode");
		return (FALSE);
	}

	//����� ������� �������� ������������
	if (!CreateSurfaces())
	{
		ErrorHandle(hWndG, "CreateSurfaces");
		return (FALSE);
	}

	//����� ������� ���������� ������������ � ������
	if (!PrepareSurfaces())
	{
		ErrorHandle(hWndG, "ConfigureSurfaces");
		return (FALSE);
	}
	return (TRUE);
}
//---------------------------------------------------------
//�������, ���������� � ������ ������
//
void ErrorHandle(HWND hwnd, LPCTSTR szError)
{
	//����������, ������� ����� ��������� ������ � ������� ������
	char szErrorMessage[255];
	//����� ��� ��� ������� ���������,
	//��������� �������� ������ DirectDraw
	RemoveDirectDraw();
	//������ �������� ����
	ShowWindow(hwnd, SW_HIDE);
	//������� ��������� �� ������
	wsprintf(szErrorMessage, "��������� ��������\n������ � %s", szError);
	MessageBox(hwnd, szErrorMessage, AppName, MB_OK);
	//��������� ����
	DestroyWindow(hwnd);
}
//---------------------------------------------------------
//�������� ������������
//
BOOL CreateSurfaces()
{
	//���������� ����������� ��� ������ ������� DirectDraw
	//�������� � ����������
	DDSURFACEDESC2 ddSurfaceDesc;
	DDSCAPS2 ddsCaps;
	HRESULT hRet;

	//�������� ��������� �� "������" � ��������� ���� �� �������
	ZeroMemory(&ddSurfaceDesc, sizeof(ddSurfaceDesc));
	ddSurfaceDesc.dwSize=sizeof(ddSurfaceDesc);

	//��������� ����������� ����� ���������
	ddSurfaceDesc.dwFlags=DDSD_CAPS | DDSD_BACKBUFFERCOUNT;
	ddSurfaceDesc.ddsCaps.dwCaps=
		DDSCAPS_PRIMARYSURFACE | DDSCAPS_FLIP | DDSCAPS_COMPLEX;
	ddSurfaceDesc.dwBackBufferCount=1;

	//�������� �����������
	hRet=pDD->CreateSurface(&ddSurfaceDesc,	&pPrimarySurface, NULL);
	if (hRet!=DD_OK)
		return (FALSE);

	//�������� ���������� ������
	ZeroMemory(&ddsCaps, sizeof(ddsCaps));
	ddsCaps.dwCaps=DDSCAPS_BACKBUFFER;
	hRet=pPrimarySurface->GetAttachedSurface(&ddsCaps, &pBackBuffer);
	if(hRet!=DD_OK)
		return (FALSE);

	//�������� ����������� ������������
	
		ZeroMemory(&ddSurfaceDesc, sizeof(ddSurfaceDesc));
		ddSurfaceDesc.dwSize=sizeof(ddSurfaceDesc);
		ddSurfaceDesc.dwFlags=DDSD_CAPS | DDSD_HEIGHT | DDSD_WIDTH;
		ddSurfaceDesc.ddsCaps.dwCaps=DDSCAPS_OFFSCREENPLAIN;
		ddSurfaceDesc.dwHeight=FRAME_HEIGHT;
		ddSurfaceDesc.dwWidth=FRAME_WIDTH;
		hRet=pDD->CreateSurface(&ddSurfaceDesc, &pPicFrames, NULL);
		if(hRet!=DD_OK)
			return (FALSE);
	
		ZeroMemory(&ddSurfaceDesc, sizeof(ddSurfaceDesc));
		ddSurfaceDesc.dwSize=sizeof(ddSurfaceDesc);
		ddSurfaceDesc.dwFlags=DDSD_CAPS |
			DDSD_HEIGHT | DDSD_WIDTH;
		ddSurfaceDesc.ddsCaps.dwCaps=DDSCAPS_OFFSCREENPLAIN;
		ddSurfaceDesc.dwHeight=FPS_HEIGHT;
		ddSurfaceDesc.dwWidth=FPS_WIDTH;
		hRet=pDD->CreateSurface(&ddSurfaceDesc, &pFPS, NULL);
		if(hRet!=DD_OK)
			return (FALSE);

	//��������� ���������� ��������� � "��������� �������"
	DDCOLORKEY ddColorKey;
	ddColorKey.dwColorSpaceLowValue=TRASPARENT_COLOR;
	ddColorKey.dwColorSpaceHighValue=TRASPARENT_COLOR;

	//��������� "�������� ������" ��� ���� ����������� ������������
	pPicFrames->SetColorKey(DDCKEY_SRCBLT, &ddColorKey);

	pFPS->SetColorKey(DDCKEY_SRCBLT, &ddColorKey);
	return (TRUE);
}
//---------------------------------------------------------
//�������� �������
//
LPDIRECTDRAWPALETTE CreateDirectDrawPalette(LPDIRECTDRAW7 pDD)
{
	//���������� ����������� � �������� ��� ������ � ��������
	LPDIRECTDRAWPALETTE pDirectDrawPal;
	PALETTEENTRY palEntries[256];
	HRESULT hRet;
	LPRGBQUAD pColorTable;
	UINT uMemNeed=sizeof(RGBQUAD)*256;
	DWORD nBytesRead;

	//�������� ������������ �����, ����������� �������
	HANDLE hFile=CreateFile(pFileName, GENERIC_READ,
			FILE_SHARE_READ, NULL, OPEN_EXISTING, 0, NULL);
	if (hFile==INVALID_HANDLE_VALUE)
	{	pDirectDrawPal=NULL;
		return (pDirectDrawPal);
	}
	//��������� ������ ��� �������� �������
	pColorTable= (LPRGBQUAD)malloc(uMemNeed);
	//��������� ��������� ����� �� ������ �������
   SetFilePointer(hFile, 
   sizeof(BITMAPFILEHEADER)+sizeof(BITMAPINFOHEADER), NULL, FILE_BEGIN);
	//������ ������� �� �����
	ReadFile(hFile, (LPVOID)pColorTable, uMemNeed, &nBytesRead, NULL);
	//�������� ������������ �����
	CloseHandle(hFile);

	//������� ������� �� RGBQUAD � RGBTRIPPLE
	for (int x=0;x<256;++x)
	{	palEntries[x].peRed=pColorTable[x].rgbRed;
		palEntries[x].peBlue=pColorTable[x].rgbBlue;
		palEntries[x].peGreen=pColorTable[x].rgbGreen;
	}
	//�������� ������� DirectDraw
	hRet=pDD->CreatePalette(DDPCAPS_8BIT | DDPCAPS_ALLOW256,
		palEntries, &pDirectDrawPal, NULL);
	if (hRet!=DD_OK) pDirectDrawPal=NULL;
	//������������ ������
	free(pColorTable);
	return (pDirectDrawPal);
}
//---------------------------------------------------------
//���������� ������������ � ������
BOOL PrepareSurfaces()
{
	//�������� ������� DirectDraw
	pDDPal=CreateDirectDrawPalette(pDD);
	if (pDDPal==NULL)
		return (FALSE);

	//������������ ������� ��������� �����������
	pPrimarySurface->SetPalette(pDDPal);

	//�������� ����������� ������ �� ������ �� ����������� �����������
	if (!LoadBMP(pPicFrames, pFileName))return (FALSE);
	
	ClearSurface(pFPS);
	return (TRUE);
}
//---------------------------------------------------------
//�������� ����������� �� BMP-�����
//
BOOL LoadBMP(LPDIRECTDRAWSURFACE7 pSurface, char* filename)
{
	//���������� ����������, ����������� ��� ������ ������ �� BMP-�����
	BYTE* pBmp;
	DWORD dwBmpSize;
	DWORD dwFileLength;
	DWORD nBytesRead;

	BITMAPINFO* pBmpInfo;
	BYTE*		pPixels;
	HDC hdc;

	//�������� ����� � ������������ �������
	HANDLE hFile=CreateFile(filename, GENERIC_READ,
			FILE_SHARE_READ, NULL,OPEN_EXISTING, 0, NULL);
	if (hFile==INVALID_HANDLE_VALUE)
		return (FALSE);

	//��������� ������� ����� � ������� ������
	dwFileLength=GetFileSize (hFile, NULL) ;
	dwBmpSize=dwFileLength-sizeof(BITMAPFILEHEADER);

	//��������� ������ ��� ������
	pBmp=	(BYTE*) malloc(dwBmpSize);
	SetFilePointer(hFile, sizeof(BITMAPFILEHEADER), NULL, FILE_BEGIN);

	//������ ����� � �������
	ReadFile(hFile, (LPVOID)pBmp, dwBmpSize, &nBytesRead, NULL);
	CloseHandle(hFile);

	pBmpInfo=(BITMAPINFO*)pBmp;
	pPixels=pBmp+sizeof(BITMAPINFOHEADER)+sizeof(RGBQUAD)*256;

	//��������� ��������� ��������� ���������� ����������� �����������
	if ((pSurface->GetDC(&hdc)) == DD_OK)
	{
	//����������� ����������� ������ �� ������
	//�� ����������� ����������� ���������� GDI
	StretchDIBits(hdc , 0, 0, FRAME_WIDTH, FRAME_HEIGHT, 0, 0, 
		   FRAME_WIDTH, FRAME_HEIGHT, pPixels, pBmpInfo, 0, SRCCOPY);
	pSurface->ReleaseDC(hdc);
	}
	//������������ ������
	free(pBmp);
	return (TRUE);
}
//---------------------------------------------------------
//�������� ����� �� "������"
void PrepareFrame()
{
	//�������� �� "������" ��������� �����������
	if (pPrimarySurface->IsLost())
	{
		//�������������� ��������� ����������� � ���������� ������
		pPrimarySurface->Restore();
		pBackBuffer->Restore();

		//�������� ����������� ������������ �� "������"
			if (pPicFrames->IsLost())
			{	//�������������� ����������� ������������
				//� ���������� �� ������� �� ������
				pPicFrames->Restore();
				LoadBMP(pPicFrames, pFileName);
			}
		
		if (pFPS->IsLost())
		{	pFPS->Restore();
			ClearSurface(pFPS);
		}
	}
}
//---------------------------------------------------------
//�������� �����������
//
BOOL ClearSurface(LPDIRECTDRAWSURFACE7 pSurface)
{
	DDSURFACEDESC2 ddSurfaceDesc;
	HRESULT hRet;

	ZeroMemory(&ddSurfaceDesc,sizeof(ddSurfaceDesc));
	ddSurfaceDesc.dwSize=sizeof(ddSurfaceDesc);

	hRet=pSurface->Lock(NULL,&ddSurfaceDesc,
			DDLOCK_SURFACEMEMORYPTR | DDLOCK_WAIT,NULL);
	if(FAILED(hRet))
	{ return (FALSE);
	}
	UINT surfaceWidth=ddSurfaceDesc.lPitch;
	UINT surfaceHeight=ddSurfaceDesc.dwHeight;

	char *buf=(char*)ddSurfaceDesc.lpSurface;
	ZeroMemory(buf,surfaceWidth*surfaceHeight);
	pSurface->Unlock(NULL);
	return (TRUE);
}
//---------------------------------------------------------
//����� ����������� �� �����
void DrawFrame()
{
	RECT rPic;
	//���������� ������������
	PrepareFrame();// �������� ����� �� ������ 
	ClearSurface(pBackBuffer);

	//��������� �������� ����������� ����� ������
	SetRect(&rPic, 0, 0, FRAME_WIDTH, FRAME_HEIGHT);

	//����������� ����������� ������ � ����������� �����������
	//�� ��������� �����
	pBackBuffer->BltFast((MAX_WIDTH-FRAME_WIDTH)/2,
	     (MAX_HEIGHT-FRAME_HEIGHT)/2,pPicFrames, &rPic, 
		 DDBLTFAST_SRCCOLORKEY | DDBLTFAST_WAIT);

	SetRect(&rPic, 0, 0, FPS_WIDTH, FPS_HEIGHT);
	pBackBuffer->BltFast((MAX_WIDTH-FPS_WIDTH)/2, 20, 
		 pFPS, &rPic, DDBLTFAST_SRCCOLORKEY | DDBLTFAST_WAIT);

	//������������ ������������
	pPrimarySurface->Flip(NULL, DDFLIP_WAIT);
}
//--------------------------------------------------
//�������� ���� �����������, ��������� � DirectDraw
void RemoveDirectDraw()
{
	//���������,    ���������� �� ��������� IDirectDraw7
	if (pDD != NULL)
	{
		//���������, ���������� �� ��������� ��������� �����������
		if (pPrimarySurface!=NULL)
		{
			//���������� ��������� ��������� �����������
			pPrimarySurface->Release();
			pPrimarySurface = NULL;
		}
		//�������� �� ������������� ����������� �����������
			if (pPicFrames!=NULL)
			{ //����������� ����������� �����������
				pPicFrames->Release();
				pPicFrames=NULL;
			}
		//�������� �� ������������� ���������� �������
		if(pDDPal!=NULL)
		{ //����������� ���������� �������
			pDDPal->Release();
			pDDPal=NULL;
		}
		//����������� ���������� DirectDraw
		pDD->Release();
		pDD=NULL;
	}
}