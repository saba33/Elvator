import React from 'react';
import { Code, Palette, TrendingUp, Image as ImageIcon, Share2, Search } from 'lucide-react';
import { useLanguage } from '../contexts/LanguageContext';

interface Service {
  icon: React.ReactNode;
  titleKey: string;
  descKey: string;
  gradient: string;
}

const Services: React.FC = () => {
  const { t } = useLanguage();

  const services: Service[] = [
    {
      icon: <Code className="w-8 h-8" />,
      titleKey: 'service_web_dev',
      descKey: 'service_web_dev_desc',
      gradient: 'from-blue-500 to-cyan-500',
    },
    {
      icon: <Palette className="w-8 h-8" />,
      titleKey: 'service_uiux',
      descKey: 'service_uiux_desc',
      gradient: 'from-pink-500 to-rose-500',
    },
    {
      icon: <TrendingUp className="w-8 h-8" />,
      titleKey: 'service_marketing',
      descKey: 'service_marketing_desc',
      gradient: 'from-green-500 to-emerald-500',
    },
    {
      icon: <ImageIcon className="w-8 h-8" />,
      titleKey: 'service_graphic',
      descKey: 'service_graphic_desc',
      gradient: 'from-orange-500 to-amber-500',
    },
    {
      icon: <Share2 className="w-8 h-8" />,
      titleKey: 'service_social',
      descKey: 'service_social_desc',
      gradient: 'from-violet-500 to-purple-500',
    },
    {
      icon: <Search className="w-8 h-8" />,
      titleKey: 'service_seo',
      descKey: 'service_seo_desc',
      gradient: 'from-teal-500 to-cyan-500',
    },
  ];

  return (
    <section id="services" className="py-20 bg-white">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="text-center mb-16">
          <h2 className="text-4xl sm:text-5xl font-bold text-gray-900 mb-4">
            {t('services_title')}
          </h2>
          <p className="text-xl text-gray-600 max-w-2xl mx-auto">
            {t('services_subtitle')}
          </p>
        </div>

        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-8">
          {services.map((service, index) => (
            <div
              key={index}
              className="group bg-white rounded-2xl p-8 shadow-lg hover:shadow-2xl transition-all duration-300 transform hover:-translate-y-2 border border-gray-100"
            >
              <div className={`w-16 h-16 bg-gradient-to-br ${service.gradient} rounded-xl flex items-center justify-center text-white mb-6 group-hover:scale-110 transition-transform duration-300`}>
                {service.icon}
              </div>
              <h3 className="text-2xl font-bold text-gray-900 mb-3">
                {t(service.titleKey)}
              </h3>
              <p className="text-gray-600 leading-relaxed">
                {t(service.descKey)}
              </p>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
};

export default Services;
