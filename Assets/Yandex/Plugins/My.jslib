/*mergeInto(LibraryManager.library, {

  SaveExtern: function (jsonData) {
    console.log("start save");
    var dateString = UTF8ToString(jsonData);
    var myObject = JSON.parse(dateString);
    player.setData(myObject);
    console.log("end save");
  },

  LoadExtern: function () {
    console.log("start load");
    player.getData().then(data =>{
      const myJSON = JSON.stringify(data);
      myGameInstance.SendMessage('Yandex', 'SetRecordsData', myJSON);

    });
    console.log("end load");
  },

});*/