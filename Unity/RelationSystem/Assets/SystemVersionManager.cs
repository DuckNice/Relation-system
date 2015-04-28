using UnityEngine;
using System.Collections;
using Google.GData.Client;
using Google.GData.Spreadsheets;
using System;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;


public class SystemVersionManager : MonoBehaviour {
    public static bool Validator(
        object sender,
    X509Certificate certificate,
    X509Chain chain,
    SslPolicyErrors policyErrors)
    {
        // Just accept and move on...

        return true;
    }


    public static void Instate()
    {
        ServicePointManager.ServerCertificateValidationCallback = Validator;
    }


    static SpreadsheetsService service = null;
    static WorksheetEntry sheet = null;
    public static Program program = null;
 
    private static void SetCell(int x, int y, string stringValue)
    {
        // Create a query for the requested cell.
        CellQuery cellQuery = new CellQuery(sheet.CellFeedLink);
        cellQuery.MinimumColumn = (uint)x;
        cellQuery.MaximumColumn = (uint)x;
        cellQuery.MinimumRow = (uint)y;
        cellQuery.MaximumRow = (uint)y;

        CellFeed cellFeed = null;

        try
        {
                // Get cells meeting the query.
            cellFeed = service.Query(cellQuery);
        }
        catch (Exception e)
        {
            print(e);
        }

        if(cellFeed != null)
                // Update the cell.
            foreach(CellEntry cellEntry in cellFeed.Entries)
            {
                cellEntry.InputValue = stringValue;
                cellEntry.Update();
            }
    }


    private static string GetCell(int x, int y)
    {
            // Create a query for the requested cell.
        CellQuery cellQuery = new CellQuery(sheet.CellFeedLink);
        cellQuery.MinimumColumn = (uint)x;
        cellQuery.MaximumColumn = (uint)x;
        cellQuery.MinimumRow = (uint)y;
        cellQuery.MaximumRow = (uint)y;

        CellFeed cellFeed = null;

        try{
                // Get cells meeting the query.
            cellFeed = service.Query(cellQuery);
         }
        catch(Exception e)
        {
            print(e);
        }

        if(cellFeed != null)
                // Update the cell.
            foreach (CellEntry cellEntry in cellFeed.Entries)
            {
              //  cellEntry.Update();
                return cellEntry.InputValue;
            }

        return "";
    }


    static void MakeSheetQuery()
    {
        WorksheetQuery worksheetsQuery = new WorksheetQuery("https://spreadsheets.google.com/feeds/worksheets/1Vak_phkztRWnnbGiMfRea8lTeq5MmLFZ3Wxs0hVN0No/private/full");
        WorksheetFeed worksheets = null;

        try
        {
            worksheets = service.Query(worksheetsQuery);
            sheet = (WorksheetEntry)worksheets.Entries[0];
        }
        catch(Exception e)
        {
            print(e);
        }
    }


    private static void AuthenticateClientLogin()
    {
        // Create the service and set user credentials.
        Instate();
        service = new SpreadsheetsService("RelationSystemVersionAccess");
        service.setUserCredentials("relationsystemtest@gmail.com", "goldenspot");
    }


    public static void PlayingVersion()
    {
        AuthenticateClientLogin();
        MakeSheetQuery();

        if(sheet != null){
            string go = GetCell(1,1);

            if(!string.IsNullOrEmpty(go)){

                SetCell(1,1, (Convert.ToInt32(go) == 2 ? 1:2).ToString());
            }
            else
            {
                print("Warning, couldn't make connection at get cell.");
            }
            
            program.playerActive = Convert.ToInt32(go) == 1 ? true:false;
        }
        else
        {
            print("Warning, couldn't make connection.");
        }

        program.shouldStart = true;
    }
}
