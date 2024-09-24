IdentityMaster

ASP.NET MVC kullanılarak geliştirilmiş, kullanıcıların kayıt olabildiği ve çeşitli doğrulama yöntemleriyle giriş yapabildiği bir kimlik yönetimi projesidir. Proje, kullanıcıların kayıt olduktan sonra e-posta onayı almalarını, Google ve Facebook gibi sosyal medya hesaplarıyla giriş yapabilmelerini ve reCAPTCHA kullanarak güvenli bir giriş deneyimi sunmalarını sağlar.

Özellikler

* Kullanıcı Kaydı ve Giriş: Kullanıcılar, e-posta adresleriyle sisteme kayıt olabilir ve giriş yapabilirler.
* E-posta Onayı: Kayıt olduktan sonra kullanıcılara bir onay e-postası gönderilir.
* Sosyal Medya Girişi: Google ve Facebook hesaplarıyla giriş yapma imkanı.
* reCAPTCHA Entegrasyonu: Botlara karşı koruma sağlamak amacıyla reCAPTCHA doğrulaması.
* Entity Framework: Proje, veritabanı işlemlerini gerçekleştirmek için Entity Framework kullanır.
* SQL Veritabanı: Kullanıcı verileri SQL veritabanında tutulur.

Kullanılan Teknolojiler

* ASP.NET MVC: Kullanıcı arayüzü ve sunucu tarafı iş mantığı için.
* Entity Framework: Veritabanı yönetimi ve işlemleri için.
* SQL Server: Kullanıcı ve kimlik doğrulama bilgilerini saklamak için.
* Google & Facebook API: Sosyal medya ile giriş işlemleri için.
* reCAPTCHA: Güvenli girişler için bot koruması.

Kurulum

* Projeyi yerel ortamınızda çalıştırmak için aşağıdaki adımları izleyin:

Projeyi Klonlayın: https://github.com/gokselkaradag/IdentityMaster.git

* Bağımlılıkları Yükleyin:

Gerekli NuGet paketlerini yükleyin: Update-Package

* Veritabanı Ayarlarını Yapılandırın:

appsettings.json veya web.config dosyasındaki veritabanı bağlantı ayarlarını kendi SQL Server ayarlarınızla güncelleyin.

* Veritabanını Oluşturun:

Entity Framework kullanarak veritabanını oluşturmak için: add-migraiton / update-database

* Google ve Facebook API Ayarları:

Google ve Facebook API için uygulama anahtarlarını alarak, projenizde ilgili alanlara ekleyin.

Gerekli OAuth ayarlarını yapın.

Uygulamayı Çalıştırın: dotnet run

Kullanım

* Kullanıcı Kayıt: Kullanıcılar, e-posta adresi ile sisteme kayıt olabilir. Kayıt sonrası onay e-postası gönderilir ve kullanıcı hesabı onaylanmadan giriş yapamaz.
* Sosyal Medya Girişi: Google ve Facebook hesaplarıyla sisteme hızlıca giriş yapılabilir.
* Güvenlik: Giriş sırasında reCAPTCHA doğrulaması yapılır.

Katkıda Bulunma

Katkılarınızı bekliyoruz! Herhangi bir hata bulursanız veya iyileştirme öneriniz varsa, lütfen bir pull request göndermekten çekinmeyin.

Lisans

Bu proje MIT Lisansı ile lisanslanmıştır. Daha fazla bilgi için LICENSE dosyasına göz atabilirsiniz.

Proje Görselleri

Aşağıda projeye ait ekran görüntülerini bulabilirsiniz:

Kayıt Ekranı

![image](https://github.com/user-attachments/assets/5e588121-b2b3-412d-82ec-6a12bd04c8a9)

Giriş Ekranı

![image](https://github.com/user-attachments/assets/da8f3524-f996-40db-b07c-c8ea0583dd4e)










