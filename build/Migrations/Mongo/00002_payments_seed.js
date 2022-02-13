
db.getCollection("payments").insertOne({
    "_id":new ObjectId("00eecb99b41848a28b6434bf"),
    "ProcessingId":"24bdb51d-c5f5-4c3b-8581-5026b987aa7a",
    "Approved":true,
    "Status":"Authorized",
    "Merchant":"merch-1234-2341",
    "Reference":"pay-202202-123456",
    "Amount":23550,
    "Currency":"eur",
    "CardToken":{
        "_id":"a93b50de30864886b4c937efdf4210df",
        "Token":"A1ADFEF4B97D3E73986CAC01F24F905429F17D4A23C401877D3DB18145888EA3",
        "NumberLast4":"7271",
        "HolderName":"John Doe",
        "ExpirationDate":{
            "Month":12,
            "Year":2022}}
});