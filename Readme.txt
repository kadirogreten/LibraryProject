* Projeyi çalıştırmadan önce package manager console üzerinden Library.Core projesi seçiniz.
* Library.API projesi set us startup project olmalıdır.
* Sırasıyla enable-migrations, add-migration ve update-database komutları çalışıtırılmalıdır.
* Son olarak swagger dökümantasyonu için xml yapısı var. bu yüzden Library.API projesine mouse sağ tuşuna basıp
properties seçildikten sonra oradan debug sekmesinden Output ve xml documentation file işaretlenmelidir.
* Swagger üzerinden Authorization key girilirken lütfen başına bearer yazıp boşuk bırakıp token yazın. (ex: bearer token)