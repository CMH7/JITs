using Google.Cloud.Firestore;

namespace JITs.Services.DB;

public interface IFDb
{
    FirestoreDb GetDb();

    CollectionReference GetCollectionReference(string  collectionName);
}
