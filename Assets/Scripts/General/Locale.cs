using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Locale : MonoBehaviour
{
    [SerializeField] private TableReference[] m_Tables;
    
    void Start()
    {
        StartCoroutine(Preload());
    }

    IEnumerator Preload()
    {
        // Tables that are not marked as Preload can be manually preloaded.
        var preloadOperation = LocalizationSettings.StringDatabase.PreloadTables(m_Tables);
        yield return preloadOperation;
    }

    public LocalizedDatabase<StringTable, StringTableEntry>.TableEntryResult GetResult(string table, string key)
    {
        StartCoroutine(Preload());
        
        // Get some text from the table, this will be immediately available now the table has been preloaded
        var result = LocalizationSettings.StringDatabase.GetTableEntryAsync(table, key).Result;
        
        return result;
    }
}
