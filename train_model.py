# train_model.py

import numpy as np
from sklearn.linear_model import LinearRegression
from skl2onnx import convert_sklearn
from skl2onnx.common.data_types import FloatTensorType
import joblib

# Generamos datos simulados de temperatura para entrenar el modelo
X = np.array([[i] for i in range(100)])  # Representa el tiempo (ej. segundos)
y = 20 + 0.05 * X.flatten() + np.random.normal(0, 0.5, 100)  # Temperatura simulada

# Entrenar el modelo de regresión lineal
model = LinearRegression()
model.fit(X, y)

# Guardar el modelo en formato ONNX
initial_type = [('float_input', FloatTensorType([None, 1]))]
onnx_model = convert_sklearn(model, initial_types=initial_type)
with open("temperature_model.onnx", "wb") as f:
    f.write(onnx_model.SerializeToString())

print("Modelo entrenado y guardado como temperature_model.onnx")
