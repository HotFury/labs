// ����: �������������� ����������� ��������� � ���������� (����)
// ������: ���. ������� ������������� ������
//"����� ����, ������������ �������� DirectDraw ���������� DirectX v.7" 
/***************************************************************************/
// �������: �� ������� �� ���������� ������� 'F1' : 
// 1 - �������  ���������� � ���������� ������������� �������� (...).
// 2 - ������� �� ����� � � ���� �������� ���� �������������� ��������� ������� � ��������� ��� �������:
//     ���������� (�� X �  �� Y), ��� � �������� ����� �������, ������� ��������� ������
/***************************************************************************/
// ����: ����������� API-��������� ������� ��� Win98/NT/2000/XP/Win2003,          
// ��������� �� 2-� �������: ������� ������� WndProc() � ������� ������� WinMain() 
/***************************************************************************/
#define WIN32_LEAN_AND_MEAN // ��� ��������� ������� ��������
#include <windows.h>        // ��� ������� � API �������� Windows
#include <stdio.h>          // ��� ������� � sprintf()
#include <ddraw.h>          // ��� ������� � DirectX
#include <mmsystem.h>       // ��� ������� � Multimedia API's DirectX
#include <winbase.h>        // ��� ������ � ����

//���������� ���������� ��� ������������� �� ���� �������� �������
static LPDIRECTDRAW7 pDirectDraw7=NULL;   // ��� pointer-� �� ����� DirectDraw
static LPDIRECTDRAWSURFACE7 pPrimarySurface=NULL;// ��� pointer-� �� ��������� ����������� (Surface)
static DDSURFACEDESC2 DDSurfaceDesc={NULL};//��������� ��� ���������� ����������� c ���������� �� ����� !!!
static HDC hDC;      //��� GDI ��������� ������ �� �����
static int nF1=0;//������� �������� ����� ������� ������� 'F1'
       HANDLE  hMyOutFile;//��� ������ � ������
static DWORD WritenBites=NULL;//���������� ���������� ����
       HRESULT hRet; //���������� ��� ������������ ��������� ����� ������
                     //[ ������������ (������) �� �����, �� ����! ]
       char Str[150],*pStr=NULL;// ��� ������������ ��������� �����
       HWND hWndG;   // ��� ������������� �������� hWnd � ������ �������� �������

/****************************************************************************/
//�������1 "��������� ������"(Callback) ��� ����������� ������ ������ DirectDrawEnumerateEx() (������������ � WndProc() )
BOOL WINAPI DDEnumCallbackEx(GUID FAR *lpGUID,//�������� GUID �������������� ������� DirectDraw 
							 LPSTR lpDriverDescription,//������ � ���������  ���������� ���������� ��������
							 LPSTR lpDriverName,//������ � ������  ���������� ���������� �������� 
							 LPVOID lpContext,//
							 HMONITOR hm //����� ������������ �������� (0 = �������) 
							)
 {  static int N=0;//��� �������� ������ �� ��� y
  //��������� ��������� ����������� (== ������� ������� ������)
    hRet=pPrimarySurface->Lock(NULL,          //��������� ��� ��������� ����������� (����� ������ RECT)
                               &DDSurfaceDesc,//����� ���������, � ������� ������� ������� ����������� ��������������� ��������� ����������� (��������� ������) - ��������� � ���������� ���������� 
                               DDLOCK_SURFACEMEMORYPTR | //���� == �������� ������� �� ������ ��������������� ����������� (��������������)
                               DDLOCK_WAIT,              //������� �� ��������� ��������� ������� � �����������  
                               NULL);         // �� ������������
  //��������� GDI ��������� ��� ������ �� ��������� �����������
    hRet=pPrimarySurface->GetDC(&hDC);
          
  //����� ������ ���������� GDI (������� ������� ��������, ���������� �������� DDEnumCallbackEx() )
    sprintf(Str,"lpGUID=%x  DriverDescription=%s  DriverName=%s  Handle to device=%x",
                 lpGUID,    lpDriverDescription,  lpDriverName,  hm);
    TextOut(hDC,20,(150+N),Str,strlen(Str));//����� ��������� ������
  //�������� ������ �� y ��� ������ ��� ��������� ������ ���� ������� ( DDEnumCallbackEx() )
    N=N+20;//20 - ��� �������� ������ �� y
  
  //������ ���� ������ � ���� (�������� \n == ������� �� ����� ������ ��������):
//  static DWORD WritenBites=NULL;//���������� ���������� ����
    sprintf(Str,"lpGUID=%x  DriverDescription=%s  DriverName=%s  Handle to device=%x \n",
                 lpGUID,    lpDriverDescription,  lpDriverName,  hm);
      WriteFile(hMyOutFile, // ���������� (�����) �� ���� (�������� ���������)
                Str,        // ����� ����������� ������
			    strlen(Str),// ������ ����������� ������
				&WritenBites,  // �������� ���������� ������� ���������� ����
				NULL           // �� ������������
               );

//	               );
	



    pPrimarySurface->ReleaseDC(hDC);//������� GDI ��������
    pPrimarySurface->Unlock(NULL);//���������� ������������ ������� � ��������� �����������

    return TRUE; // == ���������� ����� ��������� ����� 
//  return FALSE;// == ���������� ����� (���������� ����� ���� (Callback) ������� - DDEnumCallbackEx)
  };

/****************************************************************************/
//�������2 "��������� ������"(Callback) ��� ����������� ������ ������ EnumDisplayModes() (������������ � WndProc() ) 
HRESULT WINAPI EnumModesCallback2(
                LPDDSURFACEDESC2 lpddSurfaceDesc,//������� �� ��������� ���������� ��������� �� ��������� ��������� ������������  
                LPVOID lpContext //��������������� ������ �� ��������� �������(�� ���������� ���� �����) 
                				 )
   { static int K=0;//��� �������� ������ �� ��� y (� TextOut() )
     static int M=0;//��� ��������� �������
	//��������� ��������� ����������� (== ������� ������� ������)
	  hRet=pPrimarySurface->Lock(
		    NULL,//��������� ��� ����������� (����� ������ RECT)
		    &DDSurfaceDesc,//����� ��������� � ����������� �������� ��������� �����������(��������� ������)- ��������� � ���������� ���������� 
            DDLOCK_SURFACEMEMORYPTR | //���� ��������� �������� �� ������ �����������
			DDLOCK_WAIT,//������� �� ��������� ��������� ������� � �����������  
			NULL); // �� ������������
    //��������� GDI ��������� ��� ������
	  hRet=pPrimarySurface->GetDC(&hDC);

	//����� ������ ���������� GDI (������� ������� ��������, ���������� ���� �������� )
      M++;//��� ��������� �������
	  sprintf(Str,"Mode  %3d:    %4d x %4d      %2d      %3d      ",//��������� � ������� Str ������ ������ 
		                   M,                                       //�� ��������� ��������� ��������� �������� ���������� �����������
		                   lpddSurfaceDesc->dwWidth,//���������� �� X 
		                   lpddSurfaceDesc->dwHeight,//���������� �� Y
	                       lpddSurfaceDesc->ddpfPixelFormat.dwRGBBitCount,// ��� �� �������
	                       lpddSurfaceDesc->dwRefreshRate);// ������� ��������� ������ ��������
	  TextOut(hDC,20,(190+K),Str,strlen(Str));//����� ��������� ������
      K=K+20;//20 - ��� �������� ������ �� y
    
	//������ ���� ������ � ���� (�������� \n == ������� �� ����� ������ ��������):
//	  static DWORD WritenBites=NULL;//���������� ���������� ����
	  sprintf(Str,"Mode  %3d:    %4d x %4d      %2d      %3d\n",//��������� � ������� Str ������ ������ 
		                   M,                                   //�� ��������� ��������� ��������� �������� ���������� �����������
                           lpddSurfaceDesc->dwWidth,//���������� �� X
		                   lpddSurfaceDesc->dwHeight,//���������� �� Y
	                       lpddSurfaceDesc->ddpfPixelFormat.dwRGBBitCount,// ��� �� �������
                           lpddSurfaceDesc->dwRefreshRate);// ������� ��������� ������ ��������
	  // ������ ��������� ������ � ����:
	  WriteFile(hMyOutFile, // ���������� (�����) �� ���� (�������� ���������)
                Str,        // ����� ����������� ������
			    strlen(Str),// ������ ����������� ������
				&WritenBites,  // �������� ���������� ������� ���������� ����
				NULL           // �� ������������
               );

      pPrimarySurface->ReleaseDC(hDC);//������� GDI ��������
      pPrimarySurface->Unlock(NULL);//���������� ������������ ������� � ��������� �����������

	  if(Str[0]==0)return DDENUMRET_CANCEL;//���������� ����� ���� (Callback) ������� - EnumModesCallback2() 
      return DDENUMRET_OK;//���������� ����� ��������� ����� 
    } 


/****************************************************************************/
// ������� ������� == �������� ��������� �� ��������� ��������� � Windows 
LRESULT CALLBACK WndProc(HWND hWnd,UINT uMsgName,WPARAM wParam,LPARAM lParam)
{/*************************************************************************/
 switch(uMsgName) // ��������� ���������:
 /*************************************************************************/
  {case WM_KEYDOWN://��� ������� ����� �������(�� WM_CREATE �����, �.�. ���. �� ��������� ����������� ����)
    if(wParam==VK_F1)//���� ������ ������� 'F1'
     {  nF1++;//����� ������� ������� 'F1'
        pStr="DirectDraw7 �� ���������� �� ����� ������� F1 ";//��� �������� ���������� DirectDraw
	  //�������� ���������� ������� DirectDraw ���������� ����������� ������� DirectX v.7.0  
      //(�������� pointer �� DirectDraw ��������� ��� ���������� � DirectX v.7.0)
      if((nF1==1)|(nF1==2))//!!! ������ ��������� ������� DirectDraw ������ (��� ������ ���� �������� 'F1') ???
		 {  pDirectDraw7=NULL;//�������� ������� �� ��������� DirectDraw7 ����� ����������� ��� ��������
		  //�������� ���������� (== ����������) DirectDraw 
		    hRet=DirectDrawCreateEx(
				   NULL,                 // ��������� ��� �������� ������. �������� (����� ��������� ������������ ��� �� �������� !!!)
                   (VOID**)&pDirectDraw7,// �������� pointer ����� � DirectDraw7 (������ � ��� �������)
                   IID_IDirectDraw7,     // ��� ������� ���������� DirectDraw7
                   NULL);                // �� ������������
          //�������� ����� ������ �������� ���������� ������� DirectDraw  
            if(hRet==DD_OK)pStr="������ ����� ��������� ������� DirectDraw7";
            if(hRet!=DD_OK)pStr="Error ��� �������� DirectDraw7";
            if(hRet==DDERR_DIRECTDRAWALREADYCREATED)pStr="DirectDraw7 ��� ��� ������";
         }
      
       //������� ������ ������������� ������������� (���� 12 �������)
//        if(nF1<=2)//!!! ������ ������ ��� ������ ���� ��������� 'F1'
              {  hRet=pDirectDraw7->SetCooperativeLevel(hWnd,             //������ �� ��������� (������������) Windows ����
                                                        DDSCL_EXCLUSIVE | //����������t ���������� ������� ������. ��������
                                                        DDSCL_FULLSCREEN);//���� �����
                                                      //DDSCL_NORMAL);    // == Windows ���� (�������� ��� ����� WS_OVERLAPPEDWINDOW � CreateWindowEx()
                //��������:
                //��������� ������ ������������� ������������� �������� SetCooperativeLevel() 
                //������� ��� ��������� ����� � ������� ������� �������� ��������� ������ DirectDraw.
              }
//  /*
	  //������� ������������ ������ ������ �� ����� 
//     if(nF1<=2)//!!! ������ ��� ������ ���� ��������� 'F1'
            {hRet=pDirectDraw7->SetDisplayMode(1920, // �������� �� x
                                                1080, // �������� �� y
                                                 32, // ��� �� �������
                                             //  16, // ��� �� �������
                                             //   8, // ��� �� �������
                                               NULL, // ������� ������ �������
                                               NULL);// �� ������������
             //��������:
             // ���������� SetDisplayMode()���������� (��������)���� ����� ������� �������
             // (�� ����������� ��������� ���� ������ � SetDisplayMode()).
             // �������, ��� ���������� ����������� ���������� (���� SetDisplayMode())
             // ���� �������� ������ ������� F1 (== ����� ����� � ������� �������)
             // � �������� ����� DirectDraw (��� � ����� SetCooperativeLevel()). 
        } 
//  */

//!!!�������� �����������, �������������� ��������� DurectX v.7.0:
// ������� ��������� (Primary) ����������� (Surface)
static DDSURFACEDESC2 ddSurfaceDesc={NULL};//���������� ��������� ��� �������� ���������� ����������� c ���������� ��������� �������� �� ����� !!!
  //ZeroMemory(&ddSurfaceDesc, sizeof(ddSurfaceDesc));//��������� ��������� ��� ���������� �����������
  //��������� ����������� (��������) ��������� �������� ����� ��������� DDSURFACEDESC2
	ddSurfaceDesc.dwSize=sizeof(ddSurfaceDesc); //���� ������� ��������� 
	ddSurfaceDesc.dwFlags=DDSD_BACKBUFFERCOUNT | // ��������� ������������ BACKBUFFER
		                  DDSD_CAPS;             // ��������� ������������ ��������� �����������
	ddSurfaceDesc.ddsCaps.dwCaps= DDSCAPS_PRIMARYSURFACE | // ������� ������ ���� ��������� �����������
		                          DDSCAPS_COMPLEX |        // == ��������� ����������� �����������
	                              DDSCAPS_FLIP;            // ����������� Flip() ������������ ������������
	ddSurfaceDesc.dwBackBufferCount=1; // ����� ������������ 1 (����)BACKBUFFER 

	//�������� ��������� ����������� (�����������, ������� �� ������)
	hRet=pDirectDraw7->CreateSurface(
		                  &ddSurfaceDesc,//����� ��������� � ��������� ����������� �����������   
                          &pPrimarySurface,//�������� pointer �� ����������� (����� �� ������ � VRAM)
                          NULL);// �� ������������

//========================================
// ���������� ������� ������������ ������:
//========================================

//==================================
//����� �� ��������� ����������� (������� ������� ����)	
//----------------------------------
//1 ����� (���������� GDI) ������ � ����������� �������� ������
// /*
	//����������� ���������� �������� ������ (== ������� ���������� ��������� �����������) 
      hRet=pDirectDraw7->GetDisplayMode(&ddSurfaceDesc);//

    //��������� ��������� ����������� (== ������� ������� ������)
	  hRet=pPrimarySurface->Lock(
            NULL,//��������� ��� ����������� (����� ������ ����� == RECT)
		    &DDSurfaceDesc,//����� ��������������� ��������� ��� �������� ���������� �������� ��������� �����������(��������� ������)- ��������� ��������� � ���������� ���������� 
            DDLOCK_SURFACEMEMORYPTR | //���� = �������� ������� �� ������ �����������
			DDLOCK_WAIT,//������� �� ��������� ��������� ������� � �����������  
			NULL); // �� ������������
    //��������� GDI ��������� ��� ������
	  hRet=pPrimarySurface->GetDC(&hDC);// �������� ��� ��������� �����������
          
	//����� ������ ���������� GDI (������� ��������� �������� ������) 	sprintf(Str,"����� � DirectDraw-���� (��. ����):");
	  sprintf(Str,"����� � DirectDraw-���� (��. ����):");
	  TextOut(hDC,10,90,Str,strlen(Str));//����� ������ 
	  sprintf(Str,"nF1=%d",nF1);
	  TextOut(hDC,420,90,Str,strlen(Str));//����� ������� �� F1 

	  TextOut(hDC,10,110,pStr,strlen(pStr));//����� ������ �� ���������� ���������� DirectDraw
	  sprintf(Str,"pDirectDraw7=%p",pDirectDraw7);
	  TextOut(hDC,420,110,Str,strlen(Str));//����� ������ �� ���������� ���������� DirectDraw

	  sprintf(Str,"Width=%i  Height=%u  PixelFormat=%d  RefreshRate=%d ",
	               ddSurfaceDesc.dwWidth,// ���������� �� X
                                 ddSurfaceDesc.dwHeight,// ���������� �� Y
	                                    ddSurfaceDesc.ddpfPixelFormat.dwRGBBitCount,// ��� �� �������
										                 ddSurfaceDesc.dwRefreshRate);//������� ������
	  TextOut(hDC,10,130,Str,strlen(Str));//����� ������ � ������� ������
	  // �������� ��������� ������� ������������ (Surface):
	  sprintf(Str,"&ddSurfaceDesc=%p  &pPrimarySurface=%p  pPrimarySurface=%p",&ddSurfaceDesc,&pPrimarySurface,pPrimarySurface);
	  TextOut(hDC,420,130,Str,strlen(Str));//����� ������ �� ���������� &ddSurfaceDesc � &pPrimarySurface

	pPrimarySurface->ReleaseDC(hDC);//������� GDI ��������

	pPrimarySurface->Unlock(NULL);//���������� ������������ ������� � ��������� �����������

//  /*
//=!!!!=========
//��������� ���������� � ����������� �������� ���������� (� ���� ��� ����������� ��������� � �������������� � ���� ����������� (�������������) 
	//DirectDrawEnumerateEx() - ������� API (�� ������� ������������� pDirectDraw7->), ��������� (��������) � ddraw.h
	//(��� ������ DirectDrawEnumerateEx() ������������ ������� ��������� ������ DDEnumCallbackEx(), ��������� � ���� ���������)
    if(nF1==3)//������� �� �������� ������� �� F1
	  {hMyOutFile=CreateFile( //���������� ����� ��� ������ ����������� ������� �������������� �������
                                    "DisplayModes.doc",//��� ������������ ����� � �������� �������������� �������
                                     GENERIC_WRITE,     //������ - ������ ��� ������
                                   //FILE_SHARE_READ,   //��������� ������������� ������ ����� ������ ������� 
                                     0,                 //��������� ������������� ������ ����� ������ ������� 
                                     NULL,              //������� ������
                                     OPEN_ALWAYS,       //��������� ���� ������ 
                                     0,                 //����,�������� = ����� ��������� �� ���� 
                                     NULL               //�� ����������
		                    );
        DirectDrawEnumerateEx( DDEnumCallbackEx,//��� ������� ��������� ������(��� ������ ��� �� ��������� ��������� ���������� ������)
	                          NULL,//�� �������� �������������� ����������, ������������ �� ��������� ��������������� � ������� ��������� ������ ( EnumModesCallbac() )
                            //0    //�������� � ���� ��������� � �������������� � ���� ����������� (�������������)
                            //DDENUM_NONDISPLAYDEVICES 
                            //DDENUM_DETACHEDSECONDARYDEVICES 
                              DDENUM_ATTACHEDSECONDARYDEVICES//�������� � ���� ��������� � �������������� � ���� ����������� (�������������)  
							  );
       CloseHandle(hMyOutFile);// ������� ����
      }
// */

//=======================================
// ������ ��������� ��������:
//IDirectDraw7::GetCaps() ==DDCAPS.dwVidMemTotal, DDCAPS.dwVidMemTotal     ddraw.h
//IDirectDraw7::GetMonitorFrequency     ddraw.h
//IDirectDraw7::GetAvailableVidMem()    ddraw.h
//IDirectDraw7::GetDeviceIdentifier()   ddraw.h
//  ID3DXContext::GetDeviceIndex()        d3dxcore.h !!!
//  IDirectInputDevice7::GetDeviceInfo()  dinput.h

//IDirectDraw7::EnumSurfaces                ddraw.h 
//IDirectDraw7::GetGDISurface               ddraw.h
//IDirectDrawColorControl::SetColorControls ddraw.h !!!
//IDirectDrawColorControl::GetColorControls ddraw.h
//IDirectDraw7::WaitForVerticalBlank        ddraw.h
//IDirectDraw7::GetVerticalBlankStatus      ddraw.h
//IDirectDraw7::GetGDISurface               ddraw.h
//IDirectDraw7::GetSurfaceFromDC            ddraw.h  
//IDirectDrawGammaControl::GetGammaRamp     ddraw.h 
//IDirectDrawGammaControl::SetGammaRamp     ddraw.h
//IDirectDraw7::FlipToGDISurface            ddraw.h



//=!!!!==(API � Callback ��������)=======

//��������� ���������� � �������������� ��������� ������������ (���� ��� ��������� � �������������� � ���� ����������� 
	//(��� ������ EnumDisplayModes() ������������ ������� ��������� ������ EnumModesCallback2(), ��������� � ���� ���������)
       if(nF1==3)//������� �� 3-�� ������� �� F1
	   {  hMyOutFile=CreateFile( //���������� ����� ��� ������ ����������� ������� �������������� �������
                                      "DisplayModes.doc",//��� ������������ ����� � �������� �������������� �������
                                      GENERIC_WRITE,     //������ - ������ ��� ������
                                    //FILE_SHARE_READ,   //��������� ������������� ������ ����� ������ ������� 
                                      0,                 //��������� ������������� ������ ����� ������ ������� 
                                       NULL,              //������� ������
                                      OPEN_ALWAYS,       //��������� ���� ������ 
                                      0,                 //����,�������� = ����� ��������� �� ���� 
                                      NULL               //�� ����������
		                      );
	      hRet=pDirectDraw7->EnumDisplayModes(DDEDM_REFRESHRATES,//����������� � ������ � ��������� �������� ���������� ������
                                                  NULL,              //����������� ��� ������
                                                  NULL,              //�� �������� �������������� ����������, ������������ �� ��������� ��������������� � ������� ��������� ������ ( EnumModesCallback2() )
                                                  EnumModesCallback2 //��� ������� ��������� ������(��� ������ ��� �� ��������� ������������ ������
                                                 ); 
         //��������� ����
           CloseHandle(hMyOutFile);// ������� ����
	    }
//=======================================

    }//����� [if(wParam==VK_F1)...] ��������� ������� F1 
   break;//����� ��������� WM_KEYDOWN (������� �� switch �� ����� WndProc())
  /****************************************************************************/
   case WM_PAINT://��������� ����� � ������� ������� ������������� ���� � ��������� ��������� ��������
    static PAINTSTRUCT ps; //��������� ��� ������ BeginPaint()
    hDC = BeginPaint(hWnd,&ps);//  � ������ WM_PAINT
		  SetTextColor(hDC, RGB(225, 150, 0));//���� ���� ������
		  SetBkColor(hDC, RGB(0, 135, 135)); //���� ���� �������� 
		  SetBkMode(hDC, OPAQUE);      //��� �������� �� ��������
          pStr="����� � ������������ Windows-���� (������ ����� ����):";
          TextOut(hDC,10,5,pStr,strlen(pStr));//����� ������
          pStr="��� �������� ���� � ����� DirectDraw ���������� ������� ������� 'F1' (�����: Alt-F4)";
          TextOut(hDC,10,30,pStr,strlen(pStr));//����� ������
		  pStr="��� ������ DirectDraw ������ ��������� 'F1' ����� ���������� ��� (�����: Alt-F4)";
          TextOut(hDC,10,50,pStr,strlen(pStr));//����� ������
    EndPaint(hWnd,&ps); // !!! ����������� � ����� WM_PAINT
   break;
   /*************************************************************************/ 
   case WM_DESTROY: //�������� VRAM, RAM ����� ����������� ������ ���������
       if (pPrimarySurface!=NULL)
        {//���������� � VRAM ������ ��������� �����������(������� ����� ����)
           pPrimarySurface->Release();
           pPrimarySurface = NULL;
	}
         //����������� � RAM ��������� ������ DirectDraw (� heap-�)
           pDirectDraw7->Release();
           pDirectDraw7=NULL;
           PostQuitMessage(0);//������� ��������� WM_QUIT == ��������� ������
   break; 
   default: return(DefWindowProc(hWnd,uMsgName,wParam,lParam));
  }
  return 0;
} //����� ������� ������� WndProc()
/***************************************************************************/

//�������� ������� ������� == ����� ����� � Windows-������
int APIENTRY WinMain(HINSTANCE hInstance,HINSTANCE hPrevInstance,
                         LPSTR lpszCmdLine,int iCmdShow)
{ static char szWndClass[]="WndClass for PPKK_Min.cpp";//��� ������ ����
  static char szWndTitle[]="���������� ������ DirectDraw (��� DirectX7)"; 
/***************************************************************************/
  WNDCLASSEX wndclass;//��������� ��������� ��� ���������� ������ ����(12 �����) 
     wndclass.lpszClassName = szWndClass; //��� ������ ���� (�����, ��. ����)
     wndclass.lpfnWndProc = WndProc;      //��� ������� �������
     wndclass.style=CS_SAVEBITS|CS_VREDRAW|CS_HREDRAW|CS_DBLCLKS;//����� ����
     wndclass.hInstance = hInstance;      // ����� ����������� ���������� ������� ����������      
     wndclass.hIcon = LoadIcon( NULL, IDI_APPLICATION );// ������ ...
     wndclass.hIconSm = LoadIcon( NULL, IDI_QUESTION ); // ������ ...
     wndclass.hCursor = LoadCursor( NULL, IDC_ARROW );  // ����� �������
   //wndclass.hbrBackground =(HBRUSH) GetStockObject(BLACK_BRUSH);//������ ��� = NULL
     wndclass.hbrBackground =(HBRUSH)( COLOR_GRAYTEXT + 1 );//����� ��� ������� ������� ����  
   //wndclass.hbrBackground =(HBRUSH)( COLOR_WINDOW + 1 );//����� ��� ������� ������� ����
     wndclass.lpszMenuName = NULL;
     wndclass.cbClsExtra = 0;
     wndclass.cbWndExtra = 0;
     wndclass.cbSize=sizeof(wndclass);
  RegisterClassEx(&wndclass);//������  
/****************************************************************************/
  HWND hWnd=hWndG=  //����� (handle) ���� ���� � RAM (�������� ���� � RAM (��� ������ �� �����)
     CreateWindowEx(0, //WS_EX_TOPMOST, //����� ���� (��������������)
                    szWndClass,         //��� ������ ���� (�����, ��. ����)
	                szWndTitle,	        //����� ��������� � ���� (�����, ��. ����)
                    WS_POPUP,           //����� ���� ==  ��� ��������� ���������� ����
                                        //������� ������� ���� == ���� �����
				  //WS_OVERLAPPEDWINDOW,//����� ���� - ������������ ����
                  //������� �������� ������� ������� ���� (��������� �����������)
                    0,0,		//���������� X � Y ����� ����� ���� ����
                    GetSystemMetrics(SM_CXSCREEN),//������ ���� � pixel
                    GetSystemMetrics(SM_CYSCREEN),//������ ���� � pixel
                //  CW_USEDEFAULT, CW_USEDEFAULT,CW_USEDEFAULT, CW_USEDEFAULT, 
                    NULL,		//���������� ������������� ����
                    NULL,		//���������� ����
                    hInstance,  //��������� (==�����) ������� ���������� ����������
                    NULL);		//�������������� ������
/****************************************************************************/
    ShowWindow(hWnd,iCmdShow); // ����������� ���� �� ������  
    UpdateWindow(hWnd);        // ������� ������� ��������� WM_PAINT
/****************************************************************************/
   MSG msg;   // ��������� ��������� ��� ���������� ���������
   //"�����������" ���� ��������� ��������� - ������� ������� �������
   while(GetMessage(&msg,NULL,0,0))
    {TranslateMessage(&msg);
     DispatchMessage(&msg);
    }//����� �� ����� ��� ��������� ��������� ������ WM_QUIT
  return msg.wParam; 
} // ����� WinMain
