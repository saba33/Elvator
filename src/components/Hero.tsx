import React from 'react';
import { ArrowRight } from 'lucide-react';
import { useLanguage } from '../contexts/LanguageContext';

const particles = Array.from({ length: 18 }, (_, i) => ({
  left: `${(i * 53) % 100}%`,
  size: 4 + ((i * 7) % 6),
  duration: 10 + ((i * 5) % 9),
  delay: -((i * 3) % 14),
}));

const Hero: React.FC = () => {
  const { t } = useLanguage();

  const scrollToContact = () => {
    const element = document.getElementById('contact');
    if (element) {
      element.scrollIntoView({ behavior: 'smooth' });
    }
  };

  return (
    <section id="hero" className="relative overflow-hidden min-h-[75vh] flex items-center pt-16 bg-gradient-to-br from-blue-50 via-white to-gray-50">
      <div className="absolute inset-0 pointer-events-none" aria-hidden="true">
        <div className="hero-grid" />
        <div className="hero-blob w-96 h-96 bg-blue-300/40 -top-20 -left-20" />
        <div className="hero-blob w-[28rem] h-[28rem] bg-indigo-300/30 top-1/3 -right-24" style={{ animationDelay: '-6s' }} />
        <div className="hero-blob w-80 h-80 bg-sky-200/50 -bottom-16 left-1/3" style={{ animationDelay: '-12s' }} />
        {particles.map((p, i) => (
          <span
            key={i}
            className="hero-particle"
            style={{
              left: p.left,
              width: p.size,
              height: p.size,
              animationDuration: `${p.duration}s`,
              animationDelay: `${p.delay}s`,
            }}
          />
        ))}
      </div>

      <div className="relative w-full max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-16 sm:py-24">
        <div className="text-center max-w-4xl mx-auto">
          <div className="mb-8 inline-block">
            <div className="flex items-center justify-center space-x-3 bg-white px-6 py-3 rounded-full shadow-lg">
              <div className="w-12 h-12 bg-gradient-to-br from-blue-500 to-blue-700 rounded-full flex items-center justify-center">
                <span className="text-white text-2xl font-bold">G</span>
              </div>
              <h1 className="text-4xl sm:text-5xl font-bold bg-gradient-to-r from-blue-600 to-blue-800 bg-clip-text text-transparent">
                {t('hero_title')}
              </h1>
            </div>
          </div>

          <p className="text-2xl sm:text-3xl font-semibold text-gray-800 mb-6">
            {t('hero_subtitle')}
          </p>

          <p className="text-lg sm:text-xl text-gray-600 mb-10 leading-relaxed max-w-3xl mx-auto">
            {t('hero_description')}
          </p>

          <button
            onClick={scrollToContact}
            className="inline-flex items-center space-x-2 bg-blue-600 text-white px-8 py-4 rounded-full text-lg font-semibold hover:bg-blue-700 transition-all transform hover:scale-105 shadow-lg hover:shadow-xl"
          >
            <span>{t('hero_cta')}</span>
            <ArrowRight className="w-5 h-5" />
          </button>

          <div className="mt-16 grid grid-cols-2 sm:grid-cols-4 gap-8 max-w-3xl mx-auto">
            <div className="text-center">
              <div className="text-3xl sm:text-4xl font-bold text-blue-600 mb-2">3+</div>
              <div className="text-sm text-gray-600">Years</div>
            </div>
            <div className="text-center">
              <div className="text-3xl sm:text-4xl font-bold text-blue-600 mb-2">7+</div>
              <div className="text-sm text-gray-600">Projects</div>
            </div>
            <div className="text-center">
              <div className="text-3xl sm:text-4xl font-bold text-blue-600 mb-2">7+</div>
              <div className="text-sm text-gray-600">Clients</div>
            </div>
            <div className="text-center">
              <div className="text-3xl sm:text-4xl font-bold text-blue-600 mb-2">100%</div>
              <div className="text-sm text-gray-600">Satisfaction</div>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
};

export default Hero;
