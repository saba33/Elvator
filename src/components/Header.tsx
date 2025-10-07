import React, { useState } from 'react';
import { Menu, X, ArrowUp } from 'lucide-react';
import { useLanguage } from '../contexts/LanguageContext';

const Header: React.FC = () => {
  const [isMenuOpen, setIsMenuOpen] = useState(false);
  const { language, setLanguage, t } = useLanguage();

  const scrollToSection = (sectionId: string) => {
    const element = document.getElementById(sectionId);
    if (element) {
      element.scrollIntoView({ behavior: 'smooth' });
      setIsMenuOpen(false);
    }
  };

  return (
    <header className="fixed top-0 left-0 right-0 z-50 bg-white/95 backdrop-blur-sm shadow-sm">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex justify-between items-center h-16">
          <div className="flex items-center space-x-2 cursor-pointer" onClick={() => scrollToSection('hero')}>
            <ArrowUp className="w-8 h-8 text-blue-600" />
            <span className="text-2xl font-bold text-gray-900">Elevator</span>
          </div>

          <nav className="hidden md:flex items-center space-x-8">
            <button onClick={() => scrollToSection('services')} className="text-gray-700 hover:text-blue-600 transition-colors font-medium">
              {t('nav_services')}
            </button>
            <button onClick={() => scrollToSection('portfolio')} className="text-gray-700 hover:text-blue-600 transition-colors font-medium">
              {t('nav_portfolio')}
            </button>
            <button onClick={() => scrollToSection('about')} className="text-gray-700 hover:text-blue-600 transition-colors font-medium">
              {t('nav_about')}
            </button>
            <button onClick={() => scrollToSection('contact')} className="text-gray-700 hover:text-blue-600 transition-colors font-medium">
              {t('nav_contact')}
            </button>

            <div className="flex items-center space-x-2 border-l pl-6">
              <button
                onClick={() => setLanguage('en')}
                className={`px-3 py-1 rounded-md transition-colors ${
                  language === 'en' ? 'bg-blue-600 text-white' : 'text-gray-700 hover:bg-gray-100'
                }`}
              >
                EN
              </button>
              <button
                onClick={() => setLanguage('ka')}
                className={`px-3 py-1 rounded-md transition-colors ${
                  language === 'ka' ? 'bg-blue-600 text-white' : 'text-gray-700 hover:bg-gray-100'
                }`}
              >
                ქართ
              </button>
            </div>
          </nav>

          <button
            className="md:hidden text-gray-700"
            onClick={() => setIsMenuOpen(!isMenuOpen)}
          >
            {isMenuOpen ? <X className="w-6 h-6" /> : <Menu className="w-6 h-6" />}
          </button>
        </div>
      </div>

      {isMenuOpen && (
        <div className="md:hidden bg-white border-t">
          <div className="px-4 py-4 space-y-3">
            <button
              onClick={() => scrollToSection('services')}
              className="block w-full text-left px-4 py-2 text-gray-700 hover:bg-gray-100 rounded-md transition-colors"
            >
              {t('nav_services')}
            </button>
            <button
              onClick={() => scrollToSection('portfolio')}
              className="block w-full text-left px-4 py-2 text-gray-700 hover:bg-gray-100 rounded-md transition-colors"
            >
              {t('nav_portfolio')}
            </button>
            <button
              onClick={() => scrollToSection('about')}
              className="block w-full text-left px-4 py-2 text-gray-700 hover:bg-gray-100 rounded-md transition-colors"
            >
              {t('nav_about')}
            </button>
            <button
              onClick={() => scrollToSection('contact')}
              className="block w-full text-left px-4 py-2 text-gray-700 hover:bg-gray-100 rounded-md transition-colors"
            >
              {t('nav_contact')}
            </button>

            <div className="flex items-center space-x-2 pt-3 border-t">
              <button
                onClick={() => setLanguage('en')}
                className={`flex-1 px-3 py-2 rounded-md transition-colors ${
                  language === 'en' ? 'bg-blue-600 text-white' : 'text-gray-700 bg-gray-100'
                }`}
              >
                English
              </button>
              <button
                onClick={() => setLanguage('ka')}
                className={`flex-1 px-3 py-2 rounded-md transition-colors ${
                  language === 'ka' ? 'bg-blue-600 text-white' : 'text-gray-700 bg-gray-100'
                }`}
              >
                ქართული
              </button>
            </div>
          </div>
        </div>
      )}
    </header>
  );
};

export default Header;
