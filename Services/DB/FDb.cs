using Google.Cloud.Firestore;

namespace JITs.Services.DB;

public class FDb : IFDb
{
    private FirestoreDb _db;

    public FDb() => _db = FirestoreDb.Create("jits-f801f");

    public FirestoreDb GetDb() => _db;

    public CollectionReference GetCollectionReference(string collectionName) => _db.Collection(collectionName);
}
