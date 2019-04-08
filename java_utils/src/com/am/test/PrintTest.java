package com.am.test;

import java.io.FileInputStream;
import java.io.IOException;
import java.util.Date;

import javax.print.Doc;
import javax.print.DocFlavor;
import javax.print.DocPrintJob;
import javax.print.PrintException;
import javax.print.PrintService;
import javax.print.PrintServiceLookup;
import javax.print.SimpleDoc;
import javax.print.attribute.DocAttributeSet;
import javax.print.attribute.HashDocAttributeSet;
import javax.print.attribute.HashPrintRequestAttributeSet;
import javax.print.attribute.standard.Copies;
import javax.print.attribute.standard.OrientationRequested;

import com.am.utilities.datetimehelpers;
import com.am.utilities.integerhelpers;
import com.am.utilities.stringhelpers;
import com.am.utilities.tools;


public class PrintTest {

	public static void main(String[] args) {
		// 2018-07-19 16:37:52 737-main : Fax
		// 2018-07-19 16:37:52 752-main : Microsoft Print to PDF
		// 2018-07-19 16:37:52 752-main : Microsoft XPS Document Writer
		// 2018-07-19 16:37:52 753-main : NPI28A478 (HP LaserJet MFP M132snw)
		// 2018-07-19 16:37:52 753-main : Send To OneNote 2016
		// 2018-07-19 16:37:52 753-main : TOSHIBA e-STUDIO2500AC-12095745

		//drawImage("D:\\soft\\share\\1.jpg", 1);

		Date dt = datetimehelpers.toDate(2018, 1, 1, 20, 1, 1);
		
		long t =  dt.getTime()/1000;
		tools.println("utc:%d",t);
		
		byte[] dd = integerhelpers.toBuffer((int)t,true);
		tools.println(stringhelpers.bytesToHexString(dd));
		
		dt = datetimehelpers.toDate(t*1000);
		tools.println(datetimehelpers.format(dt, datetimehelpers.dateTimeFormatString));
	}

	public static void drawImage(String fileName, int count) {
		try {

			DocFlavor dof = null;
			if (fileName.endsWith(".gif")) {
				dof = DocFlavor.INPUT_STREAM.GIF;
			} else if (fileName.endsWith(".jpg")) {
				dof = DocFlavor.INPUT_STREAM.JPEG;
			} else if (fileName.endsWith(".png")) {
				dof = DocFlavor.INPUT_STREAM.PNG;
			}else if (fileName.endsWith(".bmp")) {
				dof = DocFlavor.INPUT_STREAM.AUTOSENSE;
			}

			PrintService ps = PrintServiceLookup.lookupDefaultPrintService();

			HashPrintRequestAttributeSet pras = new HashPrintRequestAttributeSet();
			// PrintRequestAttributeSet pras = new
			// HashPrintRequestAttributeSet();
			pras.add(OrientationRequested.PORTRAIT);

			pras.add(new Copies(count));

			PrintService[] pss = PrintServiceLookup.lookupPrintServices(dof, pras);
			for (PrintService p : pss) {
				if (p.getName().equals("NPI28A478 (HP LaserJet MFP M132snw)")) {
					//ps = p;
					break;
				}
				System.out.println(p.getName());
			}
			// if(true){
			// return;
			// }
			// pras.add(PrintQuality.HIGH);
			DocAttributeSet das = new HashDocAttributeSet();

			
			// 设置打印纸张的大小（以毫米为单位）
			//das.add(new MediaPrintableArea(0, 0, 210, 296, MediaPrintableArea.MM));
			FileInputStream fin = new FileInputStream(fileName);

			Doc doc = new SimpleDoc(fin, dof, das);

			DocPrintJob job = ps.createPrintJob();

			job.print(doc, pras);
			fin.close();
		} catch (IOException ie) {
			ie.printStackTrace();
		} catch (PrintException pe) {
			pe.printStackTrace();
		}
	}

}
