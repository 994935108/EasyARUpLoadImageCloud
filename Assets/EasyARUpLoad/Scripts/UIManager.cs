﻿using UnityEngine;

using System.Collections;

using System.Runtime.InteropServices;

public class UIManager : MonoBehaviour
{
    public void OpenFile() {
        OpenFileName openFileName = new OpenFileName();

        openFileName.structSize = Marshal.SizeOf(openFileName);

        openFileName.filter = "\0*.png\0*.jpg";

        openFileName.file = new string(new char[256]);

        openFileName.maxFile = openFileName.file.Length;

        openFileName.fileTitle = new string(new char[64]);

        openFileName.maxFileTitle = openFileName.fileTitle.Length;

        openFileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\');//默认路径

        openFileName.title = "窗口标题";

        openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

        if (LocalDialog.GetOpenFileName(openFileName))
        {


            UpLoadCloudManager._SInstance.UpLoadToCloud(openFileName.file);


        }
    }

}