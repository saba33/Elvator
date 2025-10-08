import React, { createContext, useContext, useState, ReactNode } from 'react';

type Language = 'en' | 'ka';

interface LanguageContextType {
  language: Language;
  setLanguage: (lang: Language) => void;
  t: (key: string) => string;
}

const translations = {
  en: {
    nav_services: 'Services',
    nav_portfolio: 'Portfolio',
    nav_about: 'About Us',
    nav_contact: 'Contact',
    hero_title: 'Elevator',
    hero_subtitle: 'Rising Toward Innovation',
    hero_description: 'Modern digital solutions that help businesses automate processes, improve management efficiency, and achieve sustainable growth.',
    hero_cta: 'Get Started',
    services_title: 'Our Services',
    services_subtitle: 'Comprehensive digital solutions for your business',
    service_web_dev: 'Web Development',
    service_web_dev_desc: 'Custom web applications built with modern technologies',
    service_uiux: 'UI/UX Design',
    service_uiux_desc: 'Beautiful and intuitive user interfaces',
    service_marketing: 'Digital Marketing',
    service_marketing_desc: 'Data-driven marketing strategies',
    service_graphic: 'Graphic Design',
    service_graphic_desc: 'Creative visual solutions',
    service_social: 'Social Media',
    service_social_desc: 'Engaging social media management',
    service_seo: 'SEO Optimization',
    service_seo_desc: 'Improve your search engine rankings',
    portfolio_title: 'Our Portfolio',
    portfolio_subtitle: 'Projects we are proud of',
    portfolio_view: 'View Project',
    about_title: 'About Elevator',
    about_content: 'Elevator is a modern technology company that has been successfully operating in the Georgian market for one year. Our mission is to create innovative digital solutions that help businesses automate processes, improve management efficiency, and achieve sustainable growth.\n\nOur team consists of experienced developers and designers dedicated to building high-quality web and software products. At Elevator, we focus on understanding client needs, ensuring security, and delivering results that truly make a difference.\n\nWithin just one year, we have earned the trust of many companies and established long-term partnerships. Our vision is to bring simplicity, speed, and innovation to Georgia\'s business landscape through technology.',
    about_tagline: 'Elevator — rising toward innovation.',
    contact_title: 'Contact Us',
    contact_subtitle: 'Let\'s work together on your next project',
    contact_name: 'Name',
    contact_email: 'Email',
    contact_phone: 'Phone (optional)',
    contact_message: 'Message',
    contact_submit: 'Send Message',
    contact_success: 'Message sent successfully!',
    contact_error: 'Error sending message. Please try again.',
    contact_info_title: 'Contact Information',
    footer_rights: '© 2024 Elevator. All rights reserved.',
  },
  ka: {
    nav_services: 'სერვისები',
    nav_portfolio: 'პორტფოლიო',
    nav_about: 'ჩვენ შესახებ',
    nav_contact: 'კონტაქტი',
    hero_title: 'ელვატორი',
    hero_subtitle: 'ზემოთ, ინოვაციისკენ',
    hero_description: 'თანამედროვე ციფრული გადაწყვეტილებები, რომლებიც ბიზნესებს ეხმარება პროცესების ავტომატიზაციაში, მართვის ეფექტურობაში და მდგრადი ზრდის მიღწევაში.',
    hero_cta: 'დაიწყეთ ახლა',
    services_title: 'ჩვენი სერვისები',
    services_subtitle: 'კომპლექსური ციფრული გადაწყვეტილებები თქვენი ბიზნესისთვის',
    service_web_dev: 'ვებ დეველოპმენტი',
    service_web_dev_desc: 'თანამედროვე ტექნოლოგიებით შექმნილი ვებ აპლიკაციები',
    service_uiux: 'UI/UX დიზაინი',
    service_uiux_desc: 'ლამაზი და ინტუიციური ინტერფეისები',
    service_marketing: 'ციფრული მარკეტინგი',
    service_marketing_desc: 'მონაცემებზე დაფუძნებული მარკეტინგული სტრატეგიები',
    service_graphic: 'გრაფიკული დიზაინი',
    service_graphic_desc: 'კრეატიული ვიზუალური გადაწყვეტილებები',
    service_social: 'სოციალური მედია',
    service_social_desc: 'მიმზიდველი სოციალური მედიის მენეჯმენტი',
    service_seo: 'SEO ოპტიმიზაცია',
    service_seo_desc: 'გააუმჯობესეთ საძიებო სისტემების რეიტინგი',
    portfolio_title: 'ჩვენი პორტფოლიო',
    portfolio_subtitle: 'პროექტები, რომლითაც ვამაყობთ',
    portfolio_view: 'პროექტის ნახვა',
    about_title: 'ელვატორის შესახებ',
    about_content: 'ელვატორი — თანამედროვე ტექნოლოგიური კომპანიაა, რომელიც უკვე ერთი წელია წარმატებით მოღვაწეობს ქართულ ბაზარზე. ჩვენი მიზანია ინოვაციური ციფრული გადაწყვეტილებების შექმნა, რომლებიც ბიზნესებს ეხმარება ავტომატიზაციაში, ეფექტურ მართვაში და ზრდის ხელშეწყობაში.\n\nჩვენი გუნდი შედგება გამოცდილი დეველოპერებისა და დიზაინერებისგან, რომლებიც ქმნიან მაღალი ხარისხის ვებ და პროგრამულ პროდუქტებს. ელვატორი განსაკუთრებულ ყურადღებას აქცევს მომხმარებლის საჭიროებებს, უსაფრთხოებასა და შედეგზე ორიენტირებულ მიდგომას.\n\nერთ წელში მოვახერხეთ მრავალი კომპანიის ნდობის მოპოვება და მათთან გრძელვადიანი თანამშრომლობის ჩამოყალიბება. ჩვენი ხედვაა — ტექნოლოგიების დახმარებით საქართველოს ბიზნესსექტორში მეტი სიმარტივე, სისწრაფე და ინოვაცია შემოვიტანოთ.',
    about_tagline: 'ელვატორი — ზემოთ, ინოვაციისკენ.',
    contact_title: 'დაგვიკავშირდით',
    contact_subtitle: 'მოდით ერთად ვიმუშაოთ თქვენს შემდეგ პროექტზე',
    contact_name: 'სახელი',
    contact_email: 'ელ-ფოსტა',
    contact_phone: 'ტელეფონი (არასავალდებულო)',
    contact_message: 'შეტყობინება',
    contact_submit: 'გაგზავნა',
    contact_success: 'შეტყობინება წარმატებით გაიგზავნა!',
    contact_error: 'შეცდომა გაგზავნისას. გთხოვთ სცადოთ თავიდან.',
    contact_info_title: 'საკონტაქტო ინფორმაცია',
    footer_rights: '© 2024 ელვატორი. ყველა უფლება დაცულია.',
  },
};

const LanguageContext = createContext<LanguageContextType | undefined>(undefined);

export const LanguageProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [language, setLanguage] = useState<Language>('ka');

  const t = (key: string): string => {
    return translations[language][key as keyof typeof translations['en']] || key;
  };

  return (
    <LanguageContext.Provider value={{ language, setLanguage, t }}>
      {children}
    </LanguageContext.Provider>
  );
};

export const useLanguage = () => {
  const context = useContext(LanguageContext);
  if (context === undefined) {
    throw new Error('useLanguage must be used within a LanguageProvider');
  }
  return context;
};
