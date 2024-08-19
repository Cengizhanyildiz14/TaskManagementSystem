# Görev Yönetim Sistemi

Bu proje, **Görev Yönetim Sistemi** için bir örnek uygulama içermektedir. Bu sistem, görev yönetimi süreçlerini dijitalleştirmeyi ve kolaylaştırmayı amaçlar. Kullanıcılar, görev atamaları yapabilir, görevleri takip edebilir ve görev durumlarını güncelleyebilir.

## Proje Hakkında

**Görev Yönetim Sistemi** projesi, görev yönetimi süreçlerini düzenlemek için tasarlanmış bir web uygulamasıdır. Kullanıcılar, görevleri oluşturabilir, atayabilir, güncelleyebilir ve silebilir. Sistem, kullanıcıların görev durumlarını takip etmelerini ve ilgili filtreler yardımıyla görevleri daha kolay yönetmelerini sağlar.

## Özellikler

- **Görev Yönetimi:** Kullanıcılar görev oluşturabilir, atayabilir ve yönetebilir.
- **Filtreleme:** Kullanıcılar, görev listesi üzerinde çeşitli filtreler kullanarak belirli görevleri arayabilir.
- **Görev Durumu:** Görevlerin durumu (Beklemede, Tamamlandı, Reddedildi) takip edilebilir.
- **Görev Onaylama/Reddetme:** Kullanıcılar kendilerine atanan görevleri onaylanabilir veya reddedilebilir.
- **Kullanıcı Profili:** Kullanıcılar profil bilgilerini görüntüleyebilir.
- **Oturum Yönetimi:**
  - **Beni Hatırla:** Giriş ekranında bulunan "Beni Hatırla" butonu ile kullanıcıların oturumları 1 gün boyunca açık kalabilir.
- **Yetkilendirme ve Yönlendirme:**
  - **İnsan Kaynakları Uzmanı** olan kişiler departman ekleyebilir, kullanıcı ekleyebilir ve kullanıcı silebilir.
  - Bu yetkiye sahip olmayan kişiler, departman ve kullanıcı yönetimi sayfalarına girmeye çalıştıklarında özel bir "access denied" sayfasına yönlendirilir.
  - Mevcut olmayan sayfalara erişim durumunda, özel bir "not found" sayfasına yönlendirilirler.
  - İlgili insan kaynakları bilgisi, kullanıcı token'ından departman bilgisi kontrol edilerek doğrulanır.
- **Bootstrap ve JavaScript Kullanımı:** Kullanıcı arayüzünde Bootstrap kullanılmış olup, bazı interaktif özellikler için JavaScript uygulanmıştır.
- **FontAwesome:** Projede ikon setleri için FontAwesome kütüphanesi kullanılmıştır.
- **Çok Katmanlı Mimari:** Proje, sürdürülebilirlik ve genişletilebilirlik için çok katmanlı bir mimari üzerine inşa edilmiştir.

## Gereksinimler

- .NET 6.0
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- Visual Studio veya Visual Studio Code
- Bootstrap
- JavaScript
- FontAwesome

## Kurulum

1. Projeyi klonlayın:
    ```sh
    git clone https://github.com/Cengizhanyildiz14/TaskManagementSystem.git
    ```

2. Proje dizinine gidin:
    ```sh
    cd TaskManagementSystem
    ```

3. Gerekli bağımlılıkları yükleyin:
    ```sh
    dotnet restore
    ```

4. **Appsettings.json Konfigürasyonu:**

   Projeyi kullanmadan önce `appsettings.json` dosyasındaki bağlantı ayarlarını ve gizli anahtarları kendi sisteminize göre düzenlemeniz gerekmektedir. Örneğin:

    ```json
    "ConnectionStrings": {
      "default": "server=YOUR_SERVER_NAME; Database=YOUR_DATABASE_NAME; integrated security=true; encrypt=false"
    },
    "ApiSettings": {
      "Secret": "YOUR_SECRET_KEY"
    }
    ```

   - **ConnectionStrings:** `server`, `Database`, ve diğer bağlantı ayarlarını kendi sisteminize göre düzenleyin.
   - **ApiSettings:** `Secret` anahtarını, güvenliğinizi sağlamak için kendi özel ve gizli bir anahtarla değiştirin.

5. Veritabanını oluşturun:
    ```sh
    dotnet ef database update
    ```

6. Uygulamayı çalıştırın:
    ```sh
    dotnet run
    ```

## Kullanım

- **Giriş Yapma:** Kullanıcılar e-posta adresleri ile sisteme giriş yapabilir.
- **Beni Hatırla:** Giriş sırasında "Beni Hatırla" butonunu seçerek, oturumunuzu 1 gün boyunca açık tutabilirsiniz.
- **Görev Oluşturma:** Ana sayfadan yeni bir görev oluşturabilir ve ilgili kullanıcılara atayabilirsiniz.
- **Görev Güncelleme ve Silme:** Kendi oluşturduğunuz görevleri güncelleyebilir veya silebilirsiniz.
- **Görev Onaylama/Reddetme:** Atanan görevleri onaylayabilir veya reddedebilirsiniz.
- **Profil Yönetimi:** Kullanıcılar profil bilgilerini görüntüleyebilir.
- **Yetkilendirme:** İnsan Kaynakları Uzmanı olan kişiler departman ve kullanıcı yönetimi işlemleri yapabilir.
- **Yönlendirme:** Yetkiniz olmayan sayfalara erişmeye çalıştığınızda veya mevcut olmayan bir sayfaya yönlendirildiğinizde, sistem sizi ilgili özel sayfalara yönlendirir.

## Ekran Görüntüleri

- **Giriş Ekranı:** Kullanıcıların sisteme giriş yaptığı ekran.

  <img src="https://github.com/user-attachments/assets/19afe322-193f-44c7-9f50-05ee77613520" alt="Giriş Ekranı" width="600"/>

- **Ana Sayfa:** Görev yönetim işlemlerinin yapıldığı ana ekran.

<img src="[https://github.com/user-attachments/assets/19afe322-193f-44c7-9f50-05ee77613520](https://github.com/user-attachments/assets/8d2941c0-5787-43f4-b316-9a3ba2f6fe76)" alt="Giriş Ekranı" width="600"/>

- **Görevlerim Sayfası:** Kullanıcıya ait görevlerin listelendiği ekran.
- **Profil Bilgilerim Sayfası:** Kullanıcının profil bilgilerini görüntüleyebildiği ekran.
- **Görev Detayları Sayfası:** Bir görevin detaylarının görüntülendiği ekran.
- **Yeni Görev Ekleme Sayfası:** Yeni bir görev oluşturma ekranı.
- **Görev Güncelleme Sayfası:** Mevcut bir görevin güncellenebildiği ekran.
- **Departman Ekleme Sayfası:** (İK Uzmanı için) Yeni departman ekleme ekranı.
- **Personel Ekleme Sayfası:** (İK Uzmanı için) Yeni personel ekleme ekranı.
- **Personel Listesi Ekranı:** (İK Uzmanı için) Sistemdeki personelin listelendiği ekran.
- **Access Denied Sayfası:** Erişim izni olmayan kullanıcılar için özel erişim engellendi sayfası.
- **Not Found Sayfası:** Mevcut olmayan bir sayfa talebi durumunda gösterilen sayfa.


## Katkıda Bulunma

Katkılarınızı bekliyoruz! Projeyi fork ederek katkıda bulunabilirsiniz. Lütfen büyük değişiklikler için önce neyi değiştirmek istediğinizi tartışmak üzere bir konu açın.

## İletişim

- **Proje Sahibi:** Cengizhan Yıldız
- **E-posta:** cengizhanyildiz.14@outlook.com
