from sqlalchemy import create_engine
from sqlalchemy.orm import scoped_session, sessionmaker
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy import Column, String, Float, Integer

engine = create_engine('sqlite:///temp.db', convert_unicode=True)
db_session = scoped_session(sessionmaker(autocommit=False,
                                         autoflush=False,
                                         bind=engine))
Base = declarative_base()
Base.query = db_session.query_property()

class Client(Base):
    __tablename__ = 'clients'
    mac = Column(String(120), primary_key=True)
    lat = Column(Float())
    lng = Column(Float())

    def __init__(self, mac=None, lat=None, lng=None):
        self.mac = mac
        self.lat = lat
        self.lng = lng

    def getObject(self):
        return {"mac": self.mac, "lat": self.lat, "lng": self.lng}

    def __repr__(self):
        return '<Client %r>' % self.mac

class Product(Base):
    __tablename__ = 'products'
    id = Column(Integer(), primary_key=True, autoincrement=True)
    lat = Column(Float())
    lng = Column(Float())
    order = Column(Integer())
    placename = Column(String())

    def __init__(self, order=None, lat=None, lng=None, pl=None):
        self.lat = lat
        self.lng = lng
        self.order = order
        self.placename = pl

    def getObject(self):
        return {"id": self.id, "order": self.order, "lat": self.lat, "lng": self.lng, "placename": self.placename}

    def __repr__(self):
        return '<Product %r>' % self.mac

def init_db():
    Base.metadata.create_all(bind=engine)