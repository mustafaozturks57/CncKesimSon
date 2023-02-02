# CncKesimSon
Muhtelif Formları Reportlar Yapıldı
-- Değişen View Son Hali 
 
  
ALTER view [dbo].[SıparısAppPoolView]  
as  
select SipID,a.Onay_fl, Kesim_fl ,PaletNo ,Palet_fl ,Pres_fl ,Paket_fl,PaketSayı,Teslim_fl,(select Fırma_Adı from Customers where id = (select MusteriId from Orfiche where FisNo=a.SipID))as MusterıAdı ,  
  (select SiparisDurumu from SıpDurumu f where f.id=c.SiparisDurumu)  as SiparisDurumu,c.Tarih,c.TerminTarihi,c.Aciklama,(SELECT MODEL FROM Items where Model= l.MalzemeKodu)as StokAdı,  
  (select MALZEME from ItemsColor where MALZEME= l.Model)as RenkAdı, Sum(l.Adet) as Adet,Sum(l.Fiyat) as Fiyat ,sum(l.m2) as m2
from SıparısAppPool a  
INNER JOIN Orfiche c ON c.FisNo=a.SipID  
INNER JOIN Orfline l ON l.OrficheNo=a.SipID  
  
group by SipID,Onay_fl,c.Id,c.Tarih,c.TerminTarihi,c.Aciklama,c.SiparisDurumu, Kesim_fl,Teslim_fl, PaletNo ,Palet_fl,PaketSayı ,Pres_fl, Paket_fl,l.MalzemeKodu,L.Model  
GO




